using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public class FakeNavigation : INavigation
    {
        public List<Page> ModalStack { get; } = new List<Page>();
        public List<Page> NavigationStack { get; } = new List<Page>();

        IReadOnlyList<Page> INavigation.ModalStack => ModalStack;

        IReadOnlyList<Page> INavigation.NavigationStack => NavigationStack;

        void INavigation.InsertPageBefore(Page page, Page before)
        {
            throw new NotImplementedException();
        }

        Task<Page> INavigation.PopAsync()
        {
            throw new NotImplementedException();
        }

        Task<Page> INavigation.PopAsync(bool animated)
        {
            throw new NotImplementedException();
        }

        Task<Page> INavigation.PopModalAsync()
        {
            throw new NotImplementedException();
        }

        Task<Page> INavigation.PopModalAsync(bool animated)
        {
            throw new NotImplementedException();
        }

        Task INavigation.PopToRootAsync()
        {
            ModalStack.Clear();
            NavigationStack.Clear();

            return Task.FromResult(0);
        }

        Task INavigation.PopToRootAsync(bool animated)
        {
            throw new NotImplementedException();
        }

        Task INavigation.PushAsync(Page page)
        {
            return Task.Run(() => NavigationStack.Add(page));
        }

        Task INavigation.PushAsync(Page page, bool animated)
        {
            throw new NotImplementedException();
        }

        Task INavigation.PushModalAsync(Page page)
        {
            return Task.Run(() => ModalStack.Add(page));
        }

        Task INavigation.PushModalAsync(Page page, bool animated)
        {
            throw new NotImplementedException();
        }

        void INavigation.RemovePage(Page page)
        {
            throw new NotImplementedException();
        }
    }
}