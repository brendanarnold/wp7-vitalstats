using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.ViewModel
{

    // A class which can be accessed in a binding as a source which accesses the main ViewModel instance,
    // Useful when in a DataContext which has no reference to the main ViewModel e.g. a ListItem

    // Instantiate in the XAML as a resource, e.g.
    //
    // <ViewModel:ViewModelLocator x:Key="ViewModelLocator" />
    // 
    // Then bind as follows
    //
    // {Binding AppViewModel.SomePropertyOnVM, Source={StaticResource ViewModelLocator}}

    public partial class ViewModelLocator
    {
        public AppViewModel AppViewModel
        {
            get { return App.VM; }
        }

    }
}
