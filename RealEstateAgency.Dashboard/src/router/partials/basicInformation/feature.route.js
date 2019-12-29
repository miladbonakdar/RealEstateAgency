
// Views basicInformation/FEATURE routes


const BasicInformationFeature = () => import('@/views/basicInformations/Feature/FeatureDetail');
const BasicInformationFeatureList = () => import('@/views/basicInformations/Feature/FeatureList');


export default
  [
    {
      path: 'feature/details/:id',
      name: 'Feature detail',
      component: BasicInformationFeature
    }
    ,
    {
      path: 'feature/list',
      name: 'Feature list',
      component: BasicInformationFeatureList
    }
  ];
