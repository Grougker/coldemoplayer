﻿using System;
using System.Threading;

namespace CDP.ViewModel
{
    class Play : Core.ViewModelBase
    {
        public Progress ProgressViewModel { get; private set; }

        private readonly Core.Demo demo;
        private readonly INavigationService navigationService = Core.ObjectCreator.Get<INavigationService>();
        private readonly Core.IDemoManager demoManager = Core.ObjectCreator.Get<Core.IDemoManager>();
        private Core.Launcher launcher;

        public Play(Core.Demo demo)
        {
            ProgressViewModel = new Progress();
            ProgressViewModel.CancelEvent += ProgressViewModel_CancelEvent;

            this.demo = demo;
            demo.ProgressChangedEvent += demo_ProgressChangedEvent;
            demo.OperationErrorEvent += demo_OperationErrorEvent;
            demo.OperationCompleteEvent += demo_OperationCompleteEvent;
            demo.OperationCancelledEvent += demo_OperationCancelledEvent;
        }

        public override void OnNavigateComplete()
        {
            launcher = demoManager.CreateLauncher(demo);

            if (!launcher.Verify())
            {
                // TODO: proper message page
                System.Windows.MessageBox.Show(launcher.Message);
                return;
            }

            ThreadPool.QueueUserWorkItem(new WaitCallback(o =>
            {
                demo.Write(launcher.CalculateDestinationFileName());
            }));
        }

        void ProgressViewModel_CancelEvent(object sender, EventArgs e)
        {
            demo.CancelOperation();
        }

        void demo_ProgressChangedEvent(object sender, Core.Demo.ProgressChangedEventArgs e)
        {
            navigationService.Invoke(new Action<int>(progress => ProgressViewModel.Value = progress), e.Progress);
        }

        void demo_OperationCompleteEvent(object sender, EventArgs e)
        {
            RemoveDemoWriteEventHandlers();
            navigationService.Invoke(new Action(() =>
            {
                navigationService.HideWindow();
                launcher.Launch();
                launcher.ProcessFound += launcher_ProcessFound;
                launcher.ProcessClosed += launcher_ProcessClosed;

                ThreadPool.QueueUserWorkItem(new WaitCallback(o =>
                {
                    launcher.MonitorProcessWorker();
                }));
            }));
        }

        void launcher_ProcessClosed(object sender, EventArgs e)
        {
            launcher.ProcessFound -= launcher_ProcessFound;
            launcher.ProcessClosed -= launcher_ProcessClosed;

            navigationService.Invoke(new Action(() =>
            {
                navigationService.ShowWindow();
                navigationService.Home();
            }));
        }

        void launcher_ProcessFound(object sender, CDP.Core.Launcher.ProcessFoundEventArgs e)
        {
        }

        void demo_OperationCancelledEvent(object sender, EventArgs e)
        {
            RemoveDemoWriteEventHandlers();
            navigationService.Invoke(new Action(() => navigationService.Home()));
        }

        void demo_OperationErrorEvent(object sender, Core.Demo.OperationErrorEventArgs e)
        {
            RemoveDemoWriteEventHandlers();
            navigationService.Invoke(new Action<string, Exception>((msg, ex) =>
            {
                //navigationService.Navigate(new View.AnalysisError(), new AnalysisError(msg, ex));
            }), e.ErrorMessage, e.Exception);
        }

        private void RemoveDemoWriteEventHandlers()
        {
            ProgressViewModel.CancelEvent -= ProgressViewModel_CancelEvent;
            demo.ProgressChangedEvent -= demo_ProgressChangedEvent;
            demo.OperationErrorEvent -= demo_OperationErrorEvent;
            demo.OperationCompleteEvent -= demo_OperationCompleteEvent;
            demo.OperationCancelledEvent -= demo_OperationCancelledEvent;
        }
    }
}