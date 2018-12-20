using Caliburn.Micro;
using CaliburnMef.Skelton.Tab;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnMef.Skelton.Shell
{

    public interface IShell { }

    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
    {
        public ShellViewModel()
        {
            //CloseStrategy = new ApplicationCloseStrategy();
            CloseStrategy = new DefaultCloseStrategy<IScreen>();
        }
        private int count = 1;
        /**
         * http://caliburnmicro.com/documentation/composition
         */
        public void OpenTab()
        {
            ActivateItem(new TabViewModel
            {
                DisplayName = "Tab " + count++
            });
        }

        public void DeactivateItemAndTryClose(IScreen item, bool close)
        {
            DeactivateItem(item, close);
            if (this.Items.Count == 0)
            {
                this.TryClose(); //No more tabs open, close window.
            }
        }

        //public void CloseItem()
        //{
        //    DeactivateItem(ActiveItem, true);
        //}
    }
}
