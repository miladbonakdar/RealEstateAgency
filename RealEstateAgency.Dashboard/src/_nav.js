export default function (lang = 'en', userAccess) {
  lang = '/' + lang;
  let navs = {
    items: []
  };


  if (!userAccess) throw new Error('access object is not valid');
  if (userAccess.dashboardAccess.accessToPage)
    navs.items.push({
      name: 'Dashboard',
      url: lang + '/dashboard',
      icon: 'icon-speedometer'
    });

  if (userAccess.estateAccess) {
    const estate = {
      name: 'Property Estates',
      icon: 'fa fa-home',
      children: []
    };

    navs.items.push({
      title: true,
      name: 'Estate',
      class: '',
      wrapper: {
        element: '',
        attributes: {}
      }
    });

    if (userAccess.estateAccess.accessToPage)
      estate.children.push({
        name: 'Estates',
        url: lang + '/estates/list',
        icon: 'fa fa-list'
      });

    if (userAccess.projectAccess.accessToPage)
      estate.children.push({
        name: 'Requests',
        url: lang + '/agent/requests',
        icon: 'fa fa-comment'
      });
    navs.items.push(estate);
  }


  if (userAccess.agentsAccess || userAccess.usersAccess) {

    if (userAccess.agentsAccess.accessToPage || userAccess.usersAccess.accessToPage)
      navs.items.push({
        title: true,
        name: 'Management',
        class: '',
        wrapper: {
          element: '',
          attributes: {}
        }
      });

    if (userAccess.agentsAccess.accessToPage)
      navs.items.push({
        name: 'Agents List',
        url: lang + '/agents/list',
        icon: 'fa fa-user-o'
      });

    if (userAccess.usersAccess.accessToPage)
      navs.items.push({
        name: 'Users List',
        url: lang + '/users/list',
        icon: 'fa fa-users'
      });
  }

  if (userAccess.basicInformationAccess.accessToPage) {
    navs.items.push({
      title: true,
      name: 'Basic information',
      class: '',
      wrapper: {
        element: '',
        attributes: {}
      }
    });
    navs.items.push({
      name: 'Basics',
      icon: 'fa fa-info',
      children: [
        {
          name: 'Property Feature',
          url: lang + '/feature/list',
          icon: 'fa fa-shower'
        },
        {
          name: 'Property Label',
          url: lang + '/label/list',
          icon: 'fa fa-tag'
        },
        {
          name: 'Property Status',
          url: lang + '/status/list',
          icon: 'fa fa-key'
        }, {
          name: 'Property Type',
          url: lang + '/type/list',
          icon: 'fa fa-hotel'
        }

        // {
        //   name: 'Important Place',
        //   url: lang + '/importantplace/list',
        //   icon: 'icon-location-pin'
        // }
      ]
    })
  }

  return navs
}
