
// login routes


const AuthLogin = () => import('@/views/pages/Login');


export default
  [
    {
      path: 'login',
      name: 'Login',
      component: AuthLogin
    }
  ];
