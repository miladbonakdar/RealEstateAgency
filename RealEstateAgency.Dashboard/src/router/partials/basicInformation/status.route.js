
// Views basicInformation/STATUS routes
const BasicInformationStatus = () => import('@/views/basicInformations/Status/StatusDetail');
const BasicInformationStatusList = () => import('@/views/basicInformations/Status/StatusList');


export default
  [
    {
      path: 'status/details/:id',
      name: 'Status detail',
      component: BasicInformationStatus
    },
    {
      path: 'status/list',
      name: 'Status list',
      component: BasicInformationStatusList
    }

  ]
