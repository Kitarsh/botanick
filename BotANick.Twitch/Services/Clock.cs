using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BotANick.Twitch.Services
{
    /// <summary>
    /// Permet d'exécuter des fonctions sur des timings régulier.
    /// </summary>
    public class Clock
    {
        /// <summary>
        /// Le timer de l'horloge.
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// La fonction qui sera exécutée à chaque fin d'horloge.
        /// </summary>
        private readonly FctToExecute _fct;

        private readonly FctAsyncToExecute _fctAsync;

        private readonly FctToExecuteAndStopClock _fctKill;

        private readonly FtcAsyncToExecuteAndStopClock _fctAsyncKill;

        /// <summary>
        /// Le nom de l'horloge.
        /// </summary>
        private readonly string Name;

        public Clock(FctToExecute fct, TimeSpan timeSpan, string name)
        {
            this._fct = fct;

            this._timer = new Timer(timeSpan.TotalMilliseconds);
            this.Name = name;
            InitClock();
        }

        public Clock(FctAsyncToExecute fct, TimeSpan timeSpan, string name)
        {
            this._fctAsync = fct;
            this._timer = new Timer(timeSpan.TotalMilliseconds);
            this.Name = name;
            InitClock();
        }

        public Clock(FctToExecuteAndStopClock fct, TimeSpan timeSpan, string name)
        {
            this._fctKill = fct;
            this._timer = new Timer(timeSpan.TotalMilliseconds);
            this.Name = name;
            InitClock();
        }

        public Clock(FtcAsyncToExecuteAndStopClock fct, TimeSpan timeSpan, string name)
        {
            this._fctAsyncKill = fct;
            this._timer = new Timer(timeSpan.TotalMilliseconds);
            this.Name = name;
            InitClock();
        }

        /// <summary>
        /// Défini le type de la fonction a exécuter.
        /// </summary>
        public delegate void FctToExecute();

        public delegate Task FctAsyncToExecute();

        public delegate bool FctToExecuteAndStopClock();

        public delegate Task<bool> FtcAsyncToExecuteAndStopClock();

        /// <summary>
        /// Exécute directement la fonction de l'horloge.
        /// </summary>
        public void Execute()
        {
            _fct();
        }

        /// <summary>
        /// Démarre l'horloge.
        /// </summary>
        public void Start()
        {
            _timer.Start();
        }

        /// <summary>
        /// Arrête l'horloge.
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }

        public string GetLog()
        {
            return $"The elapsed event on {this.Name} clock was raised at {DateTime.Now.ToString("HH:mm:ss")}";
        }

        private void InitClock()
        {
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _timer.Start();
        }

        /// <summary>
        /// Exécute la fonction stockée lorsque le timer arrive à son terme.
        /// </summary>
        /// <param name="source">La source de l'horloge.</param>
        /// <param name="e">Les paramètres de la fin du temps.</param>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine(this.GetLog());
            _fct?.Invoke();
            _fctAsync?.Invoke();

            if (_fctKill != null)
            {
                var shouldKill = _fctKill();
                if (shouldKill)
                {
                    this.Stop();
                }
            }

            if (_fctAsyncKill != null)
            {
                _ = InvokeAsyncKill();
            }
        }

        private async Task InvokeAsyncKill()
        {
            var shouldKill = await _fctAsyncKill();
            if (shouldKill)
            {
                this.Stop();
            }
        }
    }
}
