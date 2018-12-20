using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkflowDemo.Model
{
    public class Question2ViewModel : BaseViewModel
    {

        public bool Question1
        {
            get
            {
                return this.WorkflowState.Question1;
            }
            set
            {
                this.WorkflowState.Question1 = value;
                NotifyOfPropertyChange(() => Question1);
                NotifyOfPropertyChange(() => QuestionButtonLabel);
            }
        }

        public string QuestionButtonLabel
        {
            get
            {
                if (Question1)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }


        public Question2ViewModel(WorkflowState workflowState)
            : base(workflowState)
        {
            this.DisplayName = "Screen 2 - Question";
        }


        public void Next()
        {
            if (Question1)
            {
                this.NextTransition = StateTransition.Option1;
            }
            else
            {
                this.NextTransition = StateTransition.Option2;
            }
            this.TryClose();
        }
    }
}