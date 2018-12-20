using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkflowDemo.Model
{
    public class Input3ViewModel : BaseViewModel
    {
        public string Input3Field1
        {
            get
            {
                return this.WorkflowState.Input3Field1;
            }
            set
            {
                this.WorkflowState.Input3Field1 = value;
                NotifyOfPropertyChange(() => Input3Field1);
            }
        }


        public int Input3Field2
        {
            get
            {
                return this.WorkflowState.Input3Field2;
            }
            set
            {
                this.WorkflowState.Input3Field2 = value;
                NotifyOfPropertyChange(() => Input3Field2);
            }
        }


        public Input3ViewModel(WorkflowState workflowState)
            : base(workflowState)
        {
            this.DisplayName = "Screen 3 - Input";
        }


        public void Next()
        {
            this.NextTransition = StateTransition.Input3Success;
            this.TryClose();
        }
    }
}