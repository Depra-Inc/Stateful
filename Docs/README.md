# Stateful

<div align="center">
    <strong><a href="README.md">English</a> | <a href="README.RU.md">Русский</a></strong>
</div>

<details>
<summary>Table of Contents</summary>

- [Introduction](#introduction)
- [Installation](#installation)
- [How to use](#how-to-use)
    - [Implement field or property](#1-implement-a-field-or-property-of-the-state-machine)
    - [Define states](#2-define-states)
    - [Set states in your class](#3-install-states-in-your-class)
    - [Bind transitions between states](#4-bind-transitions-between-states)
    - [Start the transition system](#5-run-the-transition-system)
- [Support](#support)
- [License](#license)

</details>

## Introduction

The project provides a simple implementation of a finite state machine for use in .Net and Unity.

Finite state machines are a powerful tool for managing the behavior of objects,
states, and transitions between them.

### Features:

* Conditional transitions: You can define conditions under which an object will transition between states.
* Easy to use: The project offers a simple API for defining states and their transitions, making it easy to use even for
  beginners.
* Performance: The finite state machine implementation is optimized for high performance, making it suitable for use in
  real Unity projects.

### Contents:

| Category       | Description                                                                                    |
|----------------|------------------------------------------------------------------------------------------------|
| `Abstract`     | Project **API**, including the IState and IStateMachine interfaces.                            |
| `Finite`       | Simple finite state machine implementation providing basic functionality for state management. |
| `Transitional` | Support for transitions between states based on conditions.                                    |

## Installation

### Manual

Add the **.dll** file from the latest release to your project.

### Using NuGet

Add `Depra.Stateful` to your project using **NuGet**.

## How to use

### 1. Implement a field or property of the state machine

`TState` is a state contract. It can be any type,
but the `StateMachine` implementation works with `IState`.

```csharp
private readonly IStateMachine _stateMachine = new StateMachine();
```

```csharp
public IStateMachine<Sample> StateMachine { get; } = new StateMachine<Sample>();
```

### 2. Define states

Create states for your object.
You can override the necessary methods in the states: `Enter()`, `Exit()`.

| Method    | Info                                |
|-----------|-------------------------------------|
| `Enter()` | Called once upon entering the state |
| `Exit()`  | Called once when exiting the state  |

For example, let's create two states:

`AwaitingState` - a state for waiting for a certain condition.

```csharp
public sealed class AwaitingState : IState
{
    private readonly IFollower _follower;
    
    public AwaitingState(IFollower follower) => _follower = follower;
    
    void IState.Enter() => _follower.StopFollow();
}
```

`FollowingState` - a state for following a target.

```csharp
public sealed class FollowingState : IState
{
    private readonly Transform _target;
    private readonly IFollower _follower;
        
    public FollowingState(Transform target, IFollower follower)
    {
        _target = target;
        _follower = follower;
    }

    IState.Enter() => _follower.StartFollow(_target);
    
    IState.Exit() => _follower.StopFollow();
}
```

### 3. Install states in your class

```csharp
[RequireComponent(typeof(IFollower))]
public class Sample : MonoBehaviour
{
    [SerializeField] private Health health;
    
    public StateMachine<Sample> StateMachine { get; } = new StateMachine<Sample>();
    public Transform Target { get; set; }

    private AwaitingState _awaitingState;
    private FollowingState _followingState;
    
    private void Awake()
    {
        InstallStates();
    }

    private void InstallStates()
    {
        var follower = GetComponent<IFollower>();

        _awaitingState = new AwaitingState(follower, this);
        _followingState = new FollowingState(follower, Target, this);
    }
}
```

The arguments `startingState` and `allowReentry` are optional.
You can use the default constructor,
then you will need to set the state in `StateMachine` using the `SwitchState` method.

### 4. Bind transitions between states

We can add transitions between states.
To do this, `StatefulTransitionSystem` and `IStateTransitions` come in handy.
You can add transitions using the `Add`, `AddAny` methods, as well as the `At` and `AnyAt` extension methods.

| Method                                                       | Description                                                     |
|--------------------------------------------------------------|-----------------------------------------------------------------|
| `Add(IState from, IStateTransition transition)`              | Takes the source state and the transition                       |
| `AddAny(IStateTransition transition)`                        | Takes a transition between any states                           |
| `At(IState from, IState to, params Func<bool>[] conditions)` | Takes the source state, target state, and transition conditions |
| `AnyAt(IState to, params Func<bool>[] conditions)`           | Takes the target state and transition conditions                |

Let's consider a situation:

We want to transition from the `AwaitingState` to the `FollowingState` when the target is found.
We also want to transition back to the `AwaitingState` when the target is lost.
The `Add` method will help us with this. Let's add two transitions:

From the `AwaitingState` to the `FollowingState`:

```csharp
transitions.Add(from: _awaitingState,
    new StateTransition(to: _followingState, condition: () => _target != null));
```

Or using extension methods:

```csharp
transitions.At(from: _awaitingState, to: _followingState, () => _target != null);
```

We've added a transition from the `AwaitingState` to the `FollowingState` with a condition that checks if the target is
not null.
Why is this necessary?
If the target is null, it's logical that we can transition to the `FollowingState`.

The assembled transition matrix must be passed to the `StatefulTransitionSystem` along with the state machine:

```csharp
_transitionSystem = new StatefulTransitionSystem(_stateMachine, transitions);
```

### 5. Run the transition system

To run the `StatefulTransitionSystem`, you need to set the initial state and call the `Tick()` method in the `Update()`:

```csharp
private void Update() => _transitionSystem.Tick();
```

## Support

I am an independent developer,
and most of the development on this project is done in my spare time.
If you're interested in collaborating or hiring me for a project, check
out [my portfolio](https://github.com/Depression-aggression) and [reach out](mailto:g0dzZz1lla@yandex.ru)!

## License

**Apache-2.0**

Copyright (c) 2022-2023 Nikolay Melnikov
[g0dzZz1lla@yandex.ru](mailto:g0dzZz1lla@yandex.ru)
