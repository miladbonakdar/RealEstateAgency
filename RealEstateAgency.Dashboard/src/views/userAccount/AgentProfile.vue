<template>

<b-col xs="12" lg="12">
        <b-card>
          <b-tabs v-model="tabIndex[0]">
            <b-tab active>
              <template slot="title">
                <i class="icon-user"></i> {{tabs[0]}}
              </template>
              <InformationForm/>
            </b-tab>
            <b-tab>
              <template slot="title">
                <i class="icon-basket-loaded"></i> {{tabs[1]}}
              </template>
              <AgentInformation />
            </b-tab>
            <b-tab>
              <template slot="title">
                <i class="icon-basket-loaded"></i> {{tabs[2]}}
              </template>
              <AgentRequests />
            </b-tab>
            <b-tab>
              <template slot="title">
                <i class="icon-pie-chart"></i> {{tabs[3]}}
              </template>
              payments
            </b-tab>
          </b-tabs>
        </b-card>
      </b-col>

</template>

<script>
import InformationForm from './userProfileForms/InformationForm'
import AgentInformation from './userProfileForms/AgentInformation'
import AgentRequests from './userProfileForms/Requests'

export default {
    name: 'agentprofile',
    components:{
        InformationForm,
        AgentInformation,
        AgentRequests
    },
    data () {
    return {
      tabIndex: [0, 0],
      tabs: [
        'Account Information',
        'Agent Information',
        'Projects',
        'Payments'
      ]
    }
  }

}
</script>

<style>

</style>
