using Microsoft.Extensions.Logging;
using System.Text;
using UtilLib.Logging;

namespace UtilLib.Test.Logging
{
	public class TimeLoggerTests
	{
		[Test]
		public void TImeLogging()
		{
			var logger = new TimeLogger("Test");

			using (var s1 = logger.BeginScope("Level 1"))
			{
				Thread.Sleep(1);
				using (var s2 = s1.BeginScope("Level 2"))
				{
					Thread.Sleep(1);
					using (var s3 = s2.BeginScope("Level 3"))
					{
						Thread.Sleep(1);
					}
				}
			}

			var entries = logger.GetEntries();
			Assert.That(entries.Length, Is.EqualTo(3));
			
			// Entries are recorded in the order they complete (LIFO)
			Assert.That(entries[0].Name, Is.EqualTo("Level 3"));
			Assert.That(entries[0].Depth, Is.EqualTo(2));
			Assert.That(entries[1].Name, Is.EqualTo("Level 2"));
			Assert.That(entries[1].Depth, Is.EqualTo(1));
			Assert.That(entries[2].Name, Is.EqualTo("Level 1"));
			Assert.That(entries[2].Depth, Is.EqualTo(0));

			// Verify timing relationships
			Assert.That(entries[0].Duration, Is.LessThan(entries[1].Duration));
			Assert.That(entries[1].Duration, Is.LessThan(entries[2].Duration));
		}

		[Test]
		public void TimeLoggingWithoutUsing()
		{
			var logger = new TimeLogger("Test");

			var scope1 = logger.BeginScope("Manual Scope 1");
			Thread.Sleep(1);
			scope1.Dispose();

			var scope2 = logger.BeginScope("Manual Scope 2");
			Thread.Sleep(1);
			scope2.Dispose();

			var entries = logger.GetEntries();
			Assert.That(entries.Length, Is.EqualTo(2));
			Assert.That(entries[0].Name, Is.EqualTo("Manual Scope 1"));
			Assert.That(entries[1].Name, Is.EqualTo("Manual Scope 2"));
		}
	}
}