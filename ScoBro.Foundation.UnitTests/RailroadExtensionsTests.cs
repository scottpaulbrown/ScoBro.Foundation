using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScoBro.Foundation;
using ScoBro.Foundation.Results;
using Xunit;
using FluentAssertions;

namespace ScoBro.Foundation.Tests;

public class RailwayExtensionsTests
{
    [Fact]
    public void Then_SuccessfulResult_CallsNextFunction()
    {
        var initial = SimpleResult.Ok(5);
        var result = initial.Then(x => SimpleResult.Ok(x * 2));
        result.WasSuccessful.Should().BeTrue();
        result.Value.Should().Be(10);
    }

    [Fact]
    public void Then_FailedResult_SkipsNextFunction()
    {
        var initial = SimpleResult.Fail<int>("error");
        var result = initial.Then(x => SimpleResult.Ok(x * 2));
        result.WasSuccessful.Should().BeFalse();
        result.Errors.Should().Contain("error");
    }

    [Fact]
    public void Tap_SuccessfulResult_ExecutesSideEffect()
    {
        var initial = SimpleResult.Ok(3);
        int sideEffectValue = 0;
        var result = initial.Tap(x => sideEffectValue = x + 1);
        sideEffectValue.Should().Be(4);
        result.WasSuccessful.Should().BeTrue();
        result.Value.Should().Be(3);
    }

    [Fact]
    public void Tap_FailedResult_DoesNotExecuteSideEffect()
    {
        var initial = SimpleResult.Fail<int>("fail");
        bool sideEffectCalled = false;
        var result = initial.Tap(x => sideEffectCalled = true);
        sideEffectCalled.Should().BeFalse();
        result.WasSuccessful.Should().BeFalse();
    }

    [Fact]
    public void Map_SuccessfulResult_MapsValue()
    {
        var initial = SimpleResult.Ok(2);
        var result = initial.Map(x => x.ToString());
        result.WasSuccessful.Should().BeTrue();
        result.Value.Should().Be("2");
    }

    [Fact]
    public void Map_FailedResult_DoesNotMapValue()
    {
        var initial = SimpleResult.Fail<int>("fail");
        var result = initial.Map(x => x.ToString());
        result.WasSuccessful.Should().BeFalse();
        result.Errors.Should().Contain("fail");
    }

    [Fact]
    public void Then_NonGeneric_SuccessfulResult_CallsNextFunction()
    {
        var initial = SimpleResult.Ok();
        var result = initial.Then(() => SimpleResult.Ok(42));
        result.WasSuccessful.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Then_NonGeneric_FailedResult_SkipsNextFunction()
    {
        var initial = SimpleResult.Fail("fail");
        var result = initial.Then(() => SimpleResult.Ok(42));
        result.WasSuccessful.Should().BeFalse();
        result.Errors.Should().Contain("fail");
    }

    [Fact]
    public void Tap_NonGeneric_SuccessfulResult_ExecutesSideEffect()
    {
        var initial = SimpleResult.Ok();
        bool called = false;
        var result = initial.Tap(() => called = true);
        called.Should().BeTrue();
        result.WasSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Tap_NonGeneric_FailedResult_DoesNotExecuteSideEffect()
    {
        var initial = SimpleResult.Fail("fail");
        bool called = false;
        var result = initial.Tap(() => called = true);
        called.Should().BeFalse();
        result.WasSuccessful.Should().BeFalse();
    }

    [Fact]
    public void Map_NonGeneric_SuccessfulResult_MapsValue()
    {
        var initial = SimpleResult.Ok();
        var result = initial.Map(() => 123);
        result.WasSuccessful.Should().BeTrue();
        result.Value.Should().Be(123);
    }

    [Fact]
    public void Map_NonGeneric_FailedResult_DoesNotMapValue()
    {
        var initial = SimpleResult.Fail("fail");
        var result = initial.Map(() => 123);
        result.WasSuccessful.Should().BeFalse();
        result.Errors.Should().Contain("fail");
    }

    [Fact]
    public async Task Then_Async_SuccessfulResult_CallsNextFunction()
    {
        var initial = Task.FromResult(SimpleResult.Ok(5));
        var result = await initial.Then(async x => SimpleResult.Ok(x * 2));
        result.WasSuccessful.Should().BeTrue();
        result.Value.Should().Be(10);
    }

    [Fact]
    public async Task Then_Async_FailedResult_SkipsNextFunction()
    {
        var initial = Task.FromResult(SimpleResult.Fail<int>("fail"));
        var result = await initial.Then(async x => SimpleResult.Ok(x * 2));
        result.WasSuccessful.Should().BeFalse();
        result.Errors.Should().Contain("fail");
    }

    [Fact]
    public async Task Tap_Async_SuccessfulResult_ExecutesSideEffect()
    {
        var initial = Task.FromResult(SimpleResult.Ok(3));
        int sideEffectValue = 0;
        var result = await initial.Tap(async x => sideEffectValue = x + 1);
        sideEffectValue.Should().Be(4);
        result.WasSuccessful.Should().BeTrue();
        result.Value.Should().Be(3);
    }

    [Fact]
    public async Task Tap_Async_FailedResult_DoesNotExecuteSideEffect()
    {
        var initial = Task.FromResult(SimpleResult.Fail<int>("fail"));
        bool sideEffectCalled = false;
        var result = await initial.Tap(async x => sideEffectCalled = true);
        sideEffectCalled.Should().BeFalse();
        result.WasSuccessful.Should().BeFalse();
    }

    [Fact]
    public async Task Map_Async_SuccessfulResult_MapsValue()
    {
        var initial = Task.FromResult(SimpleResult.Ok(2));
        var result = await initial.Map(x => x.ToString());
        result.WasSuccessful.Should().BeTrue();
        result.Value.Should().Be("2");
    }

    [Fact]
    public async Task Map_Async_FailedResult_DoesNotMapValue()
    {
        var initial = Task.FromResult(SimpleResult.Fail<int>("fail"));
        var result = await initial.Map(x => x.ToString());
        result.WasSuccessful.Should().BeFalse();
        result.Errors.Should().Contain("fail");
    }

    [Fact]
    public async Task Then_NonGeneric_Async_SuccessfulResult_CallsNextAsyncGenericFunction()
    {
        var result = await Task.FromResult(SimpleResult.Ok())
            .Then(() => Task.FromResult(SimpleResult.Ok("Test")))
            .Then(v => Task.FromResult(SimpleResult.Ok(42)));

        result.WasSuccessful.Should().BeTrue();
        result.Value.Should().Be(42);
    }

}
