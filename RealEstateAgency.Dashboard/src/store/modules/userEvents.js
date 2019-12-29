import {userEvents} from '../types'

const state = {
  userEvents: {items : []},
  userActions: {items : []},
};

const getters = {
  [userEvents.getters.notCompletedEventCount]:
    state => state.userEvents.items
      .reduce((total, item) => total += item.isDone ? 0 : 1, 0),
  [userEvents.getters.notCompletedActionCount]:
    state => state.userActions.items
      .reduce((total, item) => total += item.actionIsSuccess ? 0 : 1, 0),
  [userEvents.getters.userEvents]: state => state.userEvents,
  [userEvents.getters.userActions]: state => state.userActions,
};

const mutations = {
  [userEvents.mutations.userActions]: (state, payload) => state.userActions = payload,
  [userEvents.mutations.userEvents]: (state, payload) => state.userEvents = payload,
};

let actions = {
  [userEvents.actions.loadEvents]: ({commit}, {
    $gate,
    page,
    done
  }) => {
    if (!page) page = 0;
    if (!done) done = console.log;
    $gate.requestActionFollowUp.page({pageNumber: page, pageSize: 5})
      .then(({data}) => {
        commit(userEvents.mutations.userEvents, data);
        done();
      }).catch(error => done(error));
  },
  [userEvents.actions.loadActions]: ({commit}, {
    $gate,
    page,
    done
  }) => {
    if (!page) page = 0;
    if (!done) done = console.log;
    $gate.requestAction.page({pageNumber: page, pageSize: 10})
      .then(({data}) => {
        commit(userEvents.mutations.userActions, data);
        done();
      }).catch(error => done(error));
  },
  [userEvents.actions.loadAll]: ({commit}, {
    $gate,
    done
  }) => {
    $gate.all([
      $gate.requestActionFollowUp.page({pageNumber: 0, pageSize: 5}),
      $gate.requestAction.page({pageNumber: 0, pageSize: 10})
    ]).then(([followUps, actions]) => {
      commit(userEvents.mutations.userEvents, followUps.data);
      commit(userEvents.mutations.userActions, actions.data);
      if (done) done();
    }).catch(err => {
      if (done) done(err);
    });
  },
};

export default {
  state,
  getters,
  mutations,
  actions
};
