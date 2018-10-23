﻿using ConTabs.TestData;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;
using System.Globalization;

namespace ConTabs.Tests
{
    class FormatTests
    {
        [Test]
        public void DateTimeFieldCanBeFormattedAcademic()
        {
            // Arrange
            var tableObj = Table<TestDataType>.Create();

            // Act
            tableObj.Columns[3].FormatString = "yyyy-MM-dd";
            var val = tableObj.Columns[3].StringValForCol(new DateTime(2017, 01, 13));

            // Assert
            val.ShouldBe("2017-01-13");
        }

        [Test]
        public void DateTimeFieldCanBeFormattedDotsShort()
        {
            // Arrange
            var tableObj = Table<TestDataType>.Create();

            // Act
            tableObj.Columns[3].FormatString = "d.M.yy";
            var val = tableObj.Columns[3].StringValForCol(new DateTime(2017, 01, 13));

            // Assert
            val.ShouldBe("13.1.17");
        }

        [Test]
        public void DateTimeFieldCanBeFormattedShort()
        {
            // Arrange
            var tableObj = Table<TestDataType>.Create();

            // Act
            tableObj.Columns[3].FormatString = "yy-MM-dd";
            var val = tableObj.Columns[3].StringValForCol(new DateTime(2017, 01, 13));

            // Assert
            val.ShouldBe("17-01-13");
        }

        [Test]
        public void DateTimeFieldCanBeFormattedUSCivil()
        {
            // Arrange
            var tableObj = Table<TestDataType>.Create();

            // Act
            tableObj.Columns[3].FormatString = "MM'/'dd'/'yyyy";
            var val = tableObj.Columns[3].StringValForCol(new DateTime(2017, 01, 13));

            // Assert
            val.ShouldBe("01/13/2017");
        }

        [Test]
        public void CurrencyFieldCanBeFormatted()
        {
            // Arrange
            var tableObj = Table<TestDataType>.Create();

            // Act
            tableObj.Columns[2].FormatString = "£0.00";
            var val = tableObj.Columns[2].StringValForCol(1.911M);

            // Assert
            var decimalSeparator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            val.ShouldBe("£1" + decimalSeparator + "91");
        }

        [Test]
        public void CurrencyFieldIsFormattedToDecimalPoint()
        {
            // Arrange
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            var tableObj = Table<TestDataType>.Create();

            // Act
            tableObj.Columns[2].FormatString = "£0.00";
            var val = tableObj.Columns[2].StringValForCol(1.911M);

            // Assert
            var decimalSeparator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            val.ShouldBe("£1.91");
        }

        [Test]
        public void CurrencyFieldIsFormattedToDecimalComma()
        {
            // Arrange
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("sk");
            var tableObj = Table<TestDataType>.Create();

            // Act
            tableObj.Columns[2].FormatString = "€0.00";
            var val = tableObj.Columns[2].StringValForCol(1.911M);

            // Assert
            var decimalSeparator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            val.ShouldBe("€1,91");
        }

        [Test]
        public void StringFieldNotAffectedByFormatting()
        {
            // Arrange
            var tableObj = Table<TestDataType>.Create();

            // Act
            tableObj.Columns[0].FormatString = "£0.00";
            var val = tableObj.Columns[0].StringValForCol("A");

            // Assert
            val.ShouldBe("A");
        }

        [Test]
        public void DateTimeFieldCanBeFormattedInTable()
        {
            // Arrange
            var listOfTestClasses = DataProvider.ListOfTestData(1);
            var tableObj = Table<TestDataType>.Create(listOfTestClasses);

            tableObj.Columns[0].Hide = true; // only show date field
            tableObj.Columns[1].Hide = true;
            tableObj.Columns[2].Hide = true;

            // Act
            tableObj.Columns[3].FormatString = "yy-MM-dd";
            var tableString = tableObj.ToString();

            // Assert
            string expected = "";
            expected += "+----------------+" + Environment.NewLine;
            expected += "| DateTimeColumn |" + Environment.NewLine;
            expected += "+----------------+" + Environment.NewLine;
            expected += "| 17-01-01       |" + Environment.NewLine;
            expected += "+----------------+";
            tableString.ShouldBe(expected);
        }

        [Test]
        public void DateTimeFieldCanBeFormattedInTableWithExplicitPadding()
        {
            // Arrange
            var listOfTestClasses = DataProvider.ListOfTestData(1);
            var tableObj = Table<TestDataType>.Create(listOfTestClasses);
            tableObj.Padding = new Padding(2);

            tableObj.Columns[0].Hide = true; // only show date field
            tableObj.Columns[1].Hide = true;
            tableObj.Columns[2].Hide = true;

            // Act
            tableObj.Columns[3].FormatString = "yy-MM-dd";
            var tableString = tableObj.ToString();

            // Assert
            string expected = "";
            expected += "+------------------+" + Environment.NewLine;
            expected += "|  DateTimeColumn  |" + Environment.NewLine;
            expected += "+------------------+" + Environment.NewLine;
            expected += "|  17-01-01        |" + Environment.NewLine;
            expected += "+------------------+";
            tableString.ShouldBe(expected);
        }

        [Test]
        public void CurrencyFieldCanBeFormattedInTable()
        {
            // Arrange
            var listOfTestClasses = DataProvider.ListOfTestData(2);
            var tableObj = Table<TestDataType>.Create(listOfTestClasses);

            tableObj.Columns[0].Hide = true; // only show currency field
            tableObj.Columns[1].Hide = true;
            tableObj.Columns[3].Hide = true;

            // Act
            tableObj.Columns[2].FormatString = "£0.00";
            var tableString = tableObj.ToString();

            // Assert
            var decimalSeparator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            string expected = "";
            expected += "+----------------+" + Environment.NewLine;
            expected += "| CurrencyColumn |" + Environment.NewLine;
            expected += "+----------------+" + Environment.NewLine;
            expected += "| £19" + decimalSeparator + "95         |" + Environment.NewLine;
            expected += "| -£2000" + decimalSeparator + "00      |" + Environment.NewLine;
            expected += "+----------------+";
            tableString.ShouldBe(expected);
        }

        [Test]
        public void CurrencyFieldCanBeFormattedInTableWithExplicitPadding()
        {
            // Arrange
            var listOfTestClasses = DataProvider.ListOfTestData(2);
            var tableObj = Table<TestDataType>.Create(listOfTestClasses);
            tableObj.Padding = new Padding(0);

            tableObj.Columns[0].Hide = true; // only show currency field
            tableObj.Columns[1].Hide = true;
            tableObj.Columns[3].Hide = true;

            // Act
            tableObj.Columns[2].FormatString = "£0.00";
            var tableString = tableObj.ToString();

            // Assert
            var decimalSeparator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            string expected = "";
            expected += "+--------------+" + Environment.NewLine;
            expected += "|CurrencyColumn|" + Environment.NewLine;
            expected += "+--------------+" + Environment.NewLine;
            expected += "|£19" + decimalSeparator + "95        |" + Environment.NewLine;
            expected += "|-£2000" + decimalSeparator + "00     |" + Environment.NewLine;
            expected += "+--------------+";
            tableString.ShouldBe(expected);
        }

        [Test]
        public void StringFieldNotFormattedInTable()
        {
            // Arrange
            var listOfTestClasses = DataProvider.ListOfTestData(2);
            var tableObj = Table<TestDataType>.Create(listOfTestClasses);

            tableObj.Columns[1].Hide = true; // only show string field
            tableObj.Columns[2].Hide = true;
            tableObj.Columns[3].Hide = true;

            // Act
            tableObj.Columns[0].FormatString = "£0.00";
            var tableString = tableObj.ToString();

            // Assert
            string expected = "";
            expected += "+--------------+" + Environment.NewLine;
            expected += "| StringColumn |" + Environment.NewLine;
            expected += "+--------------+" + Environment.NewLine;
            expected += "| AAAA         |" + Environment.NewLine;
            expected += "| BB           |" + Environment.NewLine;
            expected += "+--------------+";
            tableString.ShouldBe(expected);
        }

        [Test]
        public void PropWithCustomToStringNotFormattedInTable()
        {
            // Arrange
            var listOfTestClasses = DataProvider.ListOfExtendedTestData(2);
            var tableObj = Table<ExtendedTestDataType>.Create(listOfTestClasses);

            tableObj.Columns[1].Hide = true; // only show custom ToString type field
            tableObj.Columns[2].Hide = true;
            tableObj.Columns[3].Hide = true;
            tableObj.Columns[4].Hide = true;

            // Act
            tableObj.Columns[0].FormatString = "£0.00";
            var tableString = tableObj.ToString();

            // Assert
            string expected = "";
            expected += "+----------------+" + Environment.NewLine;
            expected += "| CustomToString |" + Environment.NewLine;
            expected += "+----------------+" + Environment.NewLine;
            expected += "| A              |" + Environment.NewLine;
            expected += "| B              |" + Environment.NewLine;
            expected += "+----------------+";
            tableString.ShouldBe(expected);
        }
    }
}
