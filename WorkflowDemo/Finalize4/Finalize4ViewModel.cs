using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkflowDemo.Model
{
    public class Finalize4ViewModel : BaseViewModel
    {
        private string _results;
        public string Results
        {
            get
            {
                return _results;
            }
            set
            {
                _results = value;
                NotifyOfPropertyChange(() => Results);
            }
        }


        public Finalize4ViewModel(WorkflowState workflowState)
            : base(workflowState)
        {
            DisplayName = "Screen 4 - Finalize";
            formatResults();
        }

        private void formatResults()
        {
            var builder = new StringBuilder();
            builder.AppendLine("You've finished your task! Results:");
            builder.AppendLine("Input 1/1: " + this.WorkflowState.Input1Field1);
            builder.AppendLine("Input 1/2: " + this.WorkflowState.Input1Field2);
            builder.AppendLine("Input 1/3: " + this.WorkflowState.Input1Field3);
            builder.AppendLine("Question: " + this.WorkflowState.Question1);
            if (this.WorkflowState.Question1)
            {
                builder.AppendLine("Input 3/1: " + this.WorkflowState.Input3Field1);
                builder.AppendLine("Input 3/2: " + this.WorkflowState.Input3Field2);
            }
            Results = builder.ToString();
        }

    }
}
