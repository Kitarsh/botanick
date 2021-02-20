﻿using System;
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

        public Clock(FctToExecute fct, TimeSpan timeSpan)
        {
            // Hook up the Elapsed event for the timer.
            this._timer = new Timer(timeSpan.TotalMilliseconds);
            this._fct = fct;
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _timer.Start();
        }

        public Clock(FctAsyncToExecute fct, TimeSpan timeSpan)
        {
            // Hook up the Elapsed event for the timer.
            this._timer = new Timer(timeSpan.TotalMilliseconds);
            this._fctAsync = fct;
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _timer.Start();
        }

        /// <summary>
        /// Défini le type de la fonction a exécuter.
        /// </summary>
        public delegate void FctToExecute();

        public delegate Task FctAsyncToExecute();

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

        /// <summary>
        /// Exécute la fonction stockée lorsque le timer arrive à son terme.
        /// </summary>
        /// <param name="source">La source de l'horloge.</param>
        /// <param name="e">Les paramètres de la fin du temps.</param>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
            if (_fct != null)
            {
                _fct();
            }

            if (_fctAsync != null)
            {
                _fctAsync();
            }
        }
    }
}
