// Views basicInformation/FEATURE routes


const Agent = () => import('@/views/agents/Agent');
const AgentList = () => import('@/views/agents/AgentList');


export default
  [
    {
      path: 'agents/details/:id',
      name: 'Agent detail',
      component: Agent
    }
    ,
    {
      path: 'agents/list',
      name: 'Agent list',
      component: AgentList
    }
  ];
