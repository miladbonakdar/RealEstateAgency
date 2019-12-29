
// Views basicInformation/FEATURE routes


const BasicInformationImportantPlace = () => import('@/views/basicInformations/ImportantPlace/ImportantPlaceDetail');
const BasicInformationImportantPlaceList = () => import('@/views/basicInformations/ImportantPlace/ImportantPlaceList');


export default
  [
    {
      path: 'importantPlace/details/:id',
      name: 'Important Place detail',
      component: BasicInformationImportantPlace
    }
    ,
    {
      path: 'importantPlace/list',
      name: 'Important Place list',
      component: BasicInformationImportantPlaceList
    }
  ];
