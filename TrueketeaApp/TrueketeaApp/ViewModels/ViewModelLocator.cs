using TrueketeaApp.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using TinyIoC;
using TrueketeaApp.Services.DataBase;
using TrueketeaApp.Services.MongoService;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
    public  class ViewModelLocator
    {
       
        private static TinyIoCContainer _container;
        public static DBContext sql = new DBContext();
        public static string MyName;
        public static string MyPhoto;
        public static string MyEmail;
        public static string MyId;
        public static MongoServiceDB mongo = new MongoServiceDB();
       

        static ViewModelLocator()
        {
            _container = new TinyIoCContainer();
            _container.Register<BienvenidaViewModel>();
            _container.Register<LoginViewModel>();
            _container.Register<LostPassViewModel>();
            _container.Register<RegisterViewModel>();
            _container.Register<HomeViewModel>();
            _container.Register<ValidarViewModel>();
            _container.Register<DetailPageViewModel>();
            _container.Register<AddProductViewModel>();
            _container.Register<ChatViewModel>();
            _container.Register<InformationViewModel>();
            _container.Register <IncidentReportViewModel>();
            _container.Register<ProfileViewModel>();
            _container.Register<MyArticlesViewModel>();
            _container.Register<FavViewModel>();
            _container.Register<ProfileFavViewModel>();
            _container.Register<UserInfoViewModel>();
            _container.Register<CommentsViewModel>();
            _container.Register<TruequeViewModel>();
            _container.Register<ConversationViewModel>();
            _container.Register<ChatListViewModel>();

            _container.Register<INavigationService, NavigationService>();
        }

        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }
    }
}
