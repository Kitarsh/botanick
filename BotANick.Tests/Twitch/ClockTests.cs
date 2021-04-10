using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using BotANick.Twitch.Services;
using FluentAssertions;
using System.Threading;
using System.Timers;

namespace BotANick.Tests.Twitch
{
    public class ClockTests
    {
        private static readonly TimeSpan _clockTimeSpan = new TimeSpan(0, 0, 0, 0, 500);
        private static readonly TimeSpan _timeSpanToWait = new TimeSpan(0, 0, 0, 1, 0);
        private static readonly string _clockName = "Test";

        [Fact]
        public void ShouldCallFunctionInClock()
        {
            var hasClockTicked = false;

            void ClockTicking()
            {
                hasClockTicked = true;
            }

            var clock = new Clock(ClockTicking, _clockTimeSpan, _clockName);

            clock.Execute();

            hasClockTicked.Should().BeTrue();
        }

        [Fact]
        public void ShouldCallFunctionInClockAfterTimeSpan()
        {
            var hasClockTicked = false;
            void ClockTicking()
            {
                hasClockTicked = true;
            }

            var clock = new Clock(ClockTicking, _clockTimeSpan, _clockName);

            hasClockTicked.Should().BeFalse();

            Thread.Sleep(_timeSpanToWait);

            hasClockTicked.Should().BeTrue();
        }

        [Fact]
        public void ShouldNotCallFunctionAfterTimeSpanIfStopped()
        {
            var hasClockTicked = false;
            void ClockTicking()
            {
                hasClockTicked = true;
            }

            var clock = new Clock(ClockTicking, _clockTimeSpan, _clockName);
            clock.Stop();
            hasClockTicked.Should().BeFalse();

            Thread.Sleep(_timeSpanToWait);

            hasClockTicked.Should().BeFalse();
        }

        [Fact]
        public void ShouldCallFunctionAfterTimeSpanIfRestart()
        {
            var hasClockTicked = false;
            void ClockTicking()
            {
                hasClockTicked = true;
            }

            var clock = new Clock(ClockTicking, _clockTimeSpan, _clockName);
            clock.Stop();
            clock.Start();
            hasClockTicked.Should().BeFalse();

            Thread.Sleep(_timeSpanToWait);

            hasClockTicked.Should().BeTrue();
        }

        [Fact]
        public void ShouldConsoleWrite()
        {
            static void ClockTicking()
            {
                return;
            }

            var clock = new Clock(ClockTicking, _clockTimeSpan, _clockName);
            string expectedTime = DateTime.Now.ToString("HH:mm:ss");
            var log = clock.GetLog();

            log.Should().Be($"The elapsed event on {_clockName} clock was raised at {expectedTime}");
        }
    }
}
