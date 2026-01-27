using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UtilLib.Logging
{
	public interface ITimeLogger : IDisposable
	{
		ITimeLogger BeginScope(string scopeName);

		void EndScope();
	}

	public class TimeLogger : ITimeLogger
	{
		private readonly List<TimeLogEntry> entries;
		private readonly Stack<ScopeInfo> scopeStack;
		private readonly string name;
		private readonly Stopwatch sw;

		public string Name => name;

		public bool IsRunning => sw.IsRunning;

		public TimeSpan TotalElapsed => sw.Elapsed;

		public int EntryCount => entries.Count;

		public TimeLogger(string name)
		{
			this.name = name;
			this.entries = new List<TimeLogEntry>();
			this.scopeStack = new Stack<ScopeInfo>();
			this.sw = Stopwatch.StartNew();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ITimeLogger BeginScope(string scopeName)
		{
			scopeStack.Push(new(scopeName, sw.ElapsedTicks));
			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndScope()
		{
			if (scopeStack.Count == 0)
				throw new InvalidOperationException("No active scope to end.");

			var scope = scopeStack.Pop();

			var elapsed = sw.ElapsedTicks - scope.startTicks;
			var elapsedTimeSpan = new TimeSpan(elapsed);

			var entry = new TimeLogEntry(
				scope.name,
				elapsedTimeSpan,
				scopeStack.Count,
				sw.Elapsed
			);

			entries.Add(entry);
		}

		public void Reset()
		{
			entries.Clear();
			scopeStack.Clear();
			sw.Restart();
		}

		public void Stop()
		{
			sw.Stop();
		}

		public ReadOnlySpan<TimeLogEntry> GetEntries()
		{
			return new ReadOnlySpan<TimeLogEntry>(entries.ToArray());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			EndScope();
		}
	}

	internal readonly record struct ScopeInfo(
		string name,
		long startTicks
	);

	public readonly record struct TimeLogEntry(
		string Name,
		TimeSpan Duration,
		int Depth,
		TimeSpan TotalTimeAtEnd
	);

	public static class TimeScopeExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ITimeLogger Time(this TimeLogger logger, string scopeName)
		{
			return logger.BeginScope(scopeName);
		}

		public static void WriteReport(this TimeLogger timeLogger, ILogger logger)
		{
			logger.LogInformation("=== {name} Report ===", timeLogger.Name);
			logger.LogInformation("Total Time: {time}ms", timeLogger.TotalElapsed.TotalMilliseconds);
			logger.LogInformation("Entry Count: {count}", timeLogger.EntryCount);
			logger.LogInformation("=== {text} ===", "Breakdown");

			var entries = timeLogger.GetEntries();
			var totalMs = timeLogger.TotalElapsed.TotalMilliseconds;

			if (entries.Length == 0 || totalMs <= 0)
				return;

			var maxDuration = entries.Length > 0 ? entries.ToArray().Max(e => e.Duration.TotalMilliseconds) : 0;

			// Calculate maximum width for alignment
			var maxNameWidth = 0;
			var maxDurationWidth = 0;
			foreach (var entry in entries)
			{
				var nameWithIndent = new string(' ', entry.Depth * 2) + entry.Name;
				if (nameWithIndent.Length > maxNameWidth)
					maxNameWidth = nameWithIndent.Length;

				var durationText = $"{Math.Round(entry.Duration.TotalMilliseconds, 1)}ms";
				if (durationText.Length > maxDurationWidth)
					maxDurationWidth = durationText.Length;
			}

			for (int i = entries.Length - 1; i >= 0; i--)
			{
				var entry = entries[i];

				var indent = new string(' ', entry.Depth * 2);
				var nameWithIndent = indent + entry.Name;
				var paddedName = nameWithIndent.PadRight(maxNameWidth);
				var percentage = Math.Round((entry.Duration.TotalMilliseconds / totalMs) * 100, 1);
				var barWidth = (int)Math.Round((entry.Duration.TotalMilliseconds / maxDuration) * 20);
				var bar = new string('█', barWidth) + new string('░', 20 - barWidth);
				var durationText = $"{Math.Round(entry.Duration.TotalMilliseconds, 1)}";
				var duartionPadding = new string(' ', maxDurationWidth - durationText.Length);

				logger.LogInformation("{paddedName} [{bar}] {duration}ms{durationPadding} ({percentage}%)", paddedName, bar, durationText, duartionPadding, percentage);
			}
		}
	}
}
