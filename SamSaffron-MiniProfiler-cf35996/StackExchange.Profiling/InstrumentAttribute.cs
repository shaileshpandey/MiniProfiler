using System;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace StackExchange.Profiling
{

  /// <summary>
  /// 
  /// </summary>
  public enum InsrumentationOption
  {
    /// <summary>
    /// 
    /// </summary>
    Required = 0,

    /// <summary>
    /// 
    /// </summary>
    NotNeeded = 1,

    /// <summary>
    /// 
    /// </summary>
    Skip = 2,
  }

  /// <summary>
  /// 
  /// </summary>
  [AttributeUsage(AttributeTargets.All)]
  [MulticastAttributeUsage(MulticastTargets.Method, AllowMultiple = false)]
  [Serializable]
  public sealed class InstrumentAttribute : OnMethodBoundaryAspect
  {
    private readonly InsrumentationOption option;

    /// <summary>
    /// 
    /// </summary>
    public InstrumentAttribute() : this(InsrumentationOption.Required)
    {
      
    }

    /// <summary>
    /// 
    /// </summary>
    public InstrumentAttribute(InsrumentationOption option)
    {
      this.option = option;
    }

    public override void OnEntry(MethodExecutionArgs args)
    {
      if(option == InsrumentationOption.Skip) return;
      var profiler = MiniProfiler.Current;
      if (option == InsrumentationOption.Required)
      {
        profiler = MiniProfiler.Start();
        using (profiler.Step("Entering into method " + args.Method.Name))
        {

        }

      }
      else
      {
        MiniProfiler.Stop(true);
      }

      base.OnEntry(args);
    }

    public override void OnExit(MethodExecutionArgs args)
    {
      if (option == InsrumentationOption.Skip) return;
      var profiler = MiniProfiler.Current;
      if (option == InsrumentationOption.Required)
      {
        using (profiler.Step("Exiting from method " + args.Method.Name))
        {
        }
      }
      else
      {
        MiniProfiler.Stop(true);
      }
      base.OnExit(args);
    }
  }
}
