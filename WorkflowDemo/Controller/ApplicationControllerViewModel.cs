using Caliburn.Micro;
using System;
using System.Collections.Generic;
using WorkflowDemo.Infra;

namespace WorkflowDemo.Model
{
    /**
     * Martin Fowler describes this pattern of user interface design as an Application Controller, 
     * and describes when to use it: "If the flow and navigation of your application are simple enough 
     * so that anyone can visit any scren in pretty much any order, there's little value in a Application Controller. 
     * The strength of an Application Controller comes from definite rules about the order in which pages should be 
     * visited and different views depending on the state of objects." (Patterns of Enterprise Application Architecture, 
     * p 381, Application Controller)."
     */
    public class ApplicationControllerViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly ITransitionMap _tranMap;
        public ApplicationControllerViewModel(ITransitionMap tranMap)
        {
            _tranMap = tranMap;
            initializeMap();
            activateFirstScreen();
        }

        private void activateFirstScreen()
        {
            var screen = new Input1ViewModel(new WorkflowState());
            this.ActivateItem(screen);
        }

        private void initializeMap()
        {
            _tranMap.AddTransition<Input1ViewModel, Question2ViewModel>(StateTransition.Input1Success);
            _tranMap.AddTransition<Input1ViewModel, Input1ViewModel>(StateTransition.Cancel);
            _tranMap.AddTransition<Question2ViewModel, Input3ViewModel>(StateTransition.Option1);
            _tranMap.AddTransition<Question2ViewModel, Finalize4ViewModel>(StateTransition.Option2);
            _tranMap.AddTransition<Question2ViewModel, Input1ViewModel>(StateTransition.Cancel);
            _tranMap.AddTransition<Input3ViewModel, Finalize4ViewModel>(StateTransition.Input3Success);
            _tranMap.AddTransition<Input3ViewModel, Input1ViewModel>(StateTransition.Cancel);
            _tranMap.AddTransition<Finalize4ViewModel, Input1ViewModel>(StateTransition.Cancel);
        }

        /**
         * When an item is closed and that item was the active item, the conductor must then determine 
         * which item should be activated next. By default, this is the item before the previous active 
         * item in the list. If you need to change this behavior, you can override DetermineNextItemToActivate.
         */
        protected override IScreen DetermineNextItemToActivate(IList<IScreen> list, int lastIndex)
        {
            var theScreenThatJustClosed = list[lastIndex] as BaseViewModel;
            var state = theScreenThatJustClosed.WorkflowState;
            var nextVmType = _tranMap.GetNextVmType(theScreenThatJustClosed);
            var nextVmInstance = Activator.CreateInstance(nextVmType, state);
            return nextVmInstance as IScreen;
        }
    }
}
