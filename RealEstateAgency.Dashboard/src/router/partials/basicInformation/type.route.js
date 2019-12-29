// Views basicInformation/TYPE routes

const BasicInformationType = () => import('@/views/basicInformations/Type/TypeDetail');
const BasicInformationTypeList = () => import('@/views/basicInformations/Type/TypeList');


export default
[
  {
    path: 'type/details/:id',
    name: 'Type detail',
    component: BasicInformationType
  },
  {
    path: 'type/list',
    name: 'Type list',
    component: BasicInformationTypeList
  }

]
