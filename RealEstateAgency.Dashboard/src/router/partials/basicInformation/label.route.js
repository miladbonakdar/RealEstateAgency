
// Views basicInformation/Label routes
const BasicInformationLabel = () => import('@/views/basicInformations/Label/LabelDetail');
const BasicInformationLabelList = () => import('@/views/basicInformations/Label/LabelList');


export default
  [
    {
      path: 'label/details/:id',
      name: 'Label detail',
      component: BasicInformationLabel
    },
    {
      path: 'label/list',
      name: 'Label list',
      component: BasicInformationLabelList
    }
  ];
