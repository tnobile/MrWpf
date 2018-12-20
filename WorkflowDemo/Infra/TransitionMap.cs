using Caliburn.Micro;
using System;
using System.Collections.Generic;
using WorkflowDemo.Model;

namespace WorkflowDemo.Infra
{
    /**
     * http://www.safnet.com/writing/tech/2013/12/a-single-screen-workflow-application-in-wpf.html
     */
    public class TransitionMap : ITransitionMap
    {
        private Dictionary<Type, Dictionary<StateTransition, Type>> _mapOfTypeToTransition = new Dictionary<Type, Dictionary<StateTransition, Type>>();
        //private static TransitionMap m_instance;
        //private TransitionMap() { }
        //public static TransitionMap GetInstance()
        //{
        //    if (m_instance == null)
        //    {
        //        m_instance = new TransitionMap();
        //    }
        //    return m_instance;
        //}        
        public void AddTransition<TIdentity, TResponse>(StateTransition transition)
            where TIdentity : IScreen
            where TResponse : IScreen
        {
            if (!_mapOfTypeToTransition.ContainsKey(typeof(TIdentity)))
            {
                _mapOfTypeToTransition.Add(typeof(TIdentity),
                    new Dictionary<StateTransition, Type>() { { transition, typeof(TResponse) } });
            }
            else
            {
                _mapOfTypeToTransition[typeof(TIdentity)].Add(transition, typeof(TResponse));
            }
        }
        public Type GetNextVmType(BaseViewModel screenThatClosed)
        {
            var identity = screenThatClosed.GetType();
            var transition = screenThatClosed.NextTransition;

            if (!_mapOfTypeToTransition.ContainsKey(identity))
            {
                throw new InvalidOperationException(
                    string.Format("There are no states transitions defined for state {0}", identity.ToString()));
            }

            if (!_mapOfTypeToTransition[identity].ContainsKey(transition))
            {
                throw new InvalidOperationException(
                    string.Format("There is response setup for transition {0} from screen {1}", transition.ToString(), identity.ToString()));
            }
            return _mapOfTypeToTransition[identity][transition];
        }
    }
}