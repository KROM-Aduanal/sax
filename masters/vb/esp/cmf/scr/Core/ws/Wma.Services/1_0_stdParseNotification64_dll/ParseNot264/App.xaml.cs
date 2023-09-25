using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Parse;


//namespace ParseNot264
//{
//    /// <summary>
//    /// Lógica de interacción para App.xaml
//    /// </summary>
//    public partial class App : Application
//    {

//        public App()
//        {

//            this.InitializeComponent();

//           //this.Suspending += OnSuspending;

//            ParseClient.Initialize(new ParseClient.Configuration
//            {
//                ApplicationId = "BrounieApp",
//                Server = "http://krom.brounieapps.com/parse"
//            });

//            Task task = new Task(PushKromessage);
//            task.Start();
//            task.Wait();
//            Console.ReadLine();

//        }


//        static async void PushKromessage()
//        {
//            //var push = new ParsePush();
//            //push.Channels = new List<string> { "id_ALC16-1372" };
//            //push.Alert = "Nueva actualización de la referencia ALC16-1372";
//            //push.Data = new Dictionary<string, object> { { "referencia", "ALC16-1372" } };
//            //await push.SendAsync();

//            //var push = new ParsePush();
//            //push.Channels = new List<string> { "id_32443324" };
//            ////push.Alert = "Nueva actualización de la referencia 32443324";
//            //push.Data = new Dictionary<string, object> { { "referencia", "32443324" } };
//            //await push.SendAsync();


//            //var push = new ParsePush();
//            //push.Query = ParseInstallation.Query.WhereContainedIn("channels", "id_32443324");
//            //push.Data = new Dictionary<string, object> { { "referencia", "32443324" }, { "alert", "Nueva actualización de 32443324" } };
//            //await push.SendAsync();

//            var push = new ParsePush();
//            push.Channels = new List<string> { "ABC", "DEF" };
//            push.Data = new Dictionary<string, object> { { "alert", "This is a test" } };
//            await push.SendAsync();

//            MessageBox.Show("Aplicando notificación!!");

//        }

//    }

//}

namespace ParseNot264
{
    public partial class App : Application
    {

        public App()
        {

            this.InitializeComponent();

            //this.Suspending += OnSuspending;

            //ParseClient.Initialize(new ParseClient.Configuration
            //{
            //    ApplicationId = "BrounieApp",
            //    Server = "http://krom.brounieapps.com/parse/"
            //});
            ParseClient.Initialize(new ParseClient.Configuration
            {
                ApplicationId = "BrounieApp",
                WindowsKey = "C4suYZKkyRMYPGR7fEae",
                Server = "http://krom.brounieapps.com/parse/"
            });

            Task task = new Task(PushKromessage);
            task.Start();
            task.Wait();
            Console.ReadLine();

            MessageBox.Show("Aplicando notificación!!");

        }


        //static async void PushKromessage()
        //{
        //    var push = new ParsePush();
        //    push.Query = ParseInstallation.Query.WhereEqualTo("channels", "id_32443324");
        //    push.Data = new Dictionary<string, object> {
        //      {"referencia", "32443324"},
        //      {"alert", "Nueva actualización de la referencia 32443324"}
        //    };
        //    await push.SendAsync();

        //}
        static async void PushKromessage()
        {
            ParseObject push = new ParseObject("Notificacion");
            push["referencia"] = "ALC16-2346";
            push["alert"] = "Nueva actualización de la referencia ALC16-2346";
            await push.SaveAsync();


        }

    }

};