// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using BenchmarkDotNet.Validators;

namespace Depra.Stateful.Benchmarks;

public static class Program
{
	private static void Main() =>
		BenchmarkRunner.Run(typeof(Program).Assembly, DefaultConfig.Instance
			.AddValidator(JitOptimizationsValidator.FailOnError)
			.AddJob(Job.Default.WithToolchain(InProcessNoEmitToolchain.Instance))
			.AddDiagnoser(new MemoryDiagnoser(new MemoryDiagnoserConfig()))
			.WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest)));
}