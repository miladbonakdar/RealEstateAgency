// Views estate routes
const EstatesList = () => import('@/views/estates/List');
const Estate = () => import('@/views/estates/Estate');

const EstateDescription = () => import('@/views/estates/estate/Description');
const EstateSummary = () => import('@/views/estates/estate/Summary');
const EstateLocation = () => import('@/views/estates/estate/Location');
const EstatePrice = () => import('@/views/estates/estate/Price');
const EstateFeatures = () => import('@/views/estates/estate/Features');
const EstateDetails = () => import('@/views/estates/estate/Details');
const EstateAdditionalDetails = () => import('@/views/estates/estate/AdditionalDetails');
const EstateMedia = () => import('@/views/estates/estate/Media');
const EstateFloors = () => import('@/views/estates/estate/Floors');
// const EstateContacts = () => import('@/views/estates/estate/Contacts');
// const EstatePrivateNotes = () => import('@/views/estates/estate/PrivateNotes');


export default {
  path: 'estates',
  redirect: 'estates/list',
  name: 'Estates',
  component: {
    render(c) {
      return c('router-view')
    }
  },
  children: [
    {
      path: 'list',
      name: 'Estates list',
      component: EstatesList
    },
    {
      path: 'estate/:id',
      name: '',
      component: Estate,
      redirect: 'estate/:id/summary',
      children: [
        {
          path: 'summary',
          name: 'Estate summary',
          component: EstateSummary
        },
        {
          path: 'description',
          name: 'Estate description',
          component: EstateDescription
        }, {
          path: 'location',
          name: 'Estate location',
          component: EstateLocation
        }, {
          path: 'price',
          name: 'Estate price',
          component: EstatePrice
        }, {
          path: 'features',
          name: 'Estate features',
          component: EstateFeatures
        }, {
          path: 'details',
          name: 'Estate details',
          component: EstateDetails
        }, {
          path: 'additionalDetails',
          name: 'Estate additional details',
          component: EstateAdditionalDetails
        }, {
          path: 'media',
          name: 'Estate media',
          component: EstateMedia
        }, {
          path: 'floors',
          name: 'Estate floors',
          component: EstateFloors
        },
        // {
        //   path: 'contacts',
        //   name: 'Estate contacts',
        //   component: EstateContacts
        // },{
        //   path: 'private_notes',
        //   name: 'Estate private notes',
        //   component: EstatePrivateNotes
        // }
      ]
    }
  ]
}
