﻿using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;

namespace ConTabs.Tests
{
    [TestFixture]
    public class ConTab_Table_BasicTests
    {
        [Test]
        public void TableObjectCanBeConstructedWithoutData()
        {
            // Act
            var table = Table<string>.Create();

            // Assert
            table.ToString().Length.ShouldBeGreaterThan(0);
        }

        [Test]
        public void TableObjectCanBeConstructedWithData()
        {
            // Arrange
            var listOfStrings = new List<string>();

            // Act
            var table = Table<string>.Create(listOfStrings);

            // Assert
            table.ToString().Length.ShouldBeGreaterThan(0);
        }

        [Test]
        public void TableObjectHasExpectedNumberOfColumns()
        {
            // Arrange
            var listOfTestClasses = TestData.ListOfTestData();

            // Act
            var table = Table<TestDataType>.Create(listOfTestClasses);

            // Assert
            table.Columns.Count.ShouldBe(4);
        }

    }

    
}
