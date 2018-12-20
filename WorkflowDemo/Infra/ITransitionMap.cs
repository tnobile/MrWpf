using Caliburn.Micro;
using System;
using WorkflowDemo.Model;
namespace WorkflowDemo.Infra
{
    public interface ITransitionMap
    {
        void AddTransition<TIdentity, TResponse>(StateTransition transition)
            where TIdentity : IScreen
            where TResponse : IScreen;

        Type GetNextVmType(BaseViewModel screenThatClosed);
    }
}
