﻿using System;
using FluentAssertions;
using Xunit;

namespace Ehb.Dijlezonen.Kassa.App.Android.Tests
{
    public class Tests
    {
        [Fact]
        public void ShouldAlwaysPass()
        {
            Console.WriteLine("test1");
            true.Should().BeTrue();
        }
    }
}