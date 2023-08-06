## Оглавление

<details>
<summary>Навигация</summary>

* [О проекте](#о-проекте)
* [Установка](#установка)
* [Как использовать](#как-использовать)
    * [Реализуйте поле или свойство](#1-реализуйте-поле-или-свойство-istatemachinetstate)
    * [Определите состояния](#2-определите-состояния)
    * [Установите состояния в своем классе](#3-установите-состояния-в-своем-классе)
    * [Привяжите переходы между состояниями](#4-привяжите-переходы-между-состояниями)
    * [Запустите систему переходов](#5-запустите-statefultransitionsystem)

</details>
<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Введение

`Stateful` - это проект, который предоставляет простую реализацию конечного автомата для использования .Net и Unity.
Конечные автоматы являются мощным инструментом для управления поведением объектов, состояниями и переходами между ними.

### Особенности:

* Поддержка переходов по условиям: Вы можете определить условия, при выполнении которых объект будет переходить между
  состояниями.
* Простое использование: Проект предоставляет простой `API` для определения состояний и их переходов, что делает его
  легким в использовании даже для новичков.
* Производительность: Реализация конечного автомата оптимизирована для высокой производительности, что делает его
  подходящим для использования в реальных проектах `Unity`.

### Содержание:

| Категория      | Описание                                                                                                                            |
|----------------|-------------------------------------------------------------------------------------------------------------------------------------|
| `Abstract`     | В этой папке находится `API` проекта, включающее интерфейсы `IState` и `IStateMachine`.                                             |
| `Finite`       | В этой папке содержится простая реализация конечного автомата, предоставляющая базовую функциональность для управления состояниями. |
| `Transitional` | В этой папке расположен конечный автомат с поддержкой переходов по условиям.                                                        |

## Установка

Добавьте файлы .dll из последнего релиза в свой проект `Unity`.

## Как использовать

### 1. Реализуйте поле или свойство `IStateMachine<TState>`

`TState` - некое состояние. Может быть любым типом, но реализация `StateMachine` работает с `IState`.

```csharp
public readonly IStateMachine<Sample> _stateMachine = new StateMachine<Sample>();
```

```csharp
public IStateMachine<Sample> StateMachine { get; } = new StateMachine<Sample>();
```

### 2. Определите состояния

Создайте состояния для объекта. В состояниях можно переопределить нужные вам методы: `Enter()`, `Exit()`.

| Метод     | Описание                                    |
|-----------|---------------------------------------------|
| `Enter()` | Вызывается один раз при входе в состояние   |
| `Exit()`  | Вызывается один раз при выходе из состояния |

Например, создадим два состояния:

`AwaitingState` - состояние для ожидания какого-то условия.

```csharp
public sealed class AwaitingState : IState
{
    private readonly IFollower _follower;
    
    public AwaitingState(IFollower follower) => _follower = follower;
    
    void IState.Enter() => _follower.StopFollow();
}
```

`FollowingState` - состояние для следования за какой-то целью.

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

### 3. Установите состояния в своем классе

```csharp
[RequireComponent(typeof(IFollower))]
internal sealed class Sample : MonoBehaviour
{
    [SerializedField] private Transform _target;
    
    private IStateMachine _stateMachine;
    
    private void Awake() => InstallStates();

    private void InstallStates()
    {
        var follower = GetComponent<IFollower>();
        var awaitingState = new AwaitingState(follower);
        _stateMachine = new StateMachine(startingState: awaitingState, allowReentry: false);
    }
}
```

Аргументы `startingState` и `allowReentry` необязательны.
Вы можете использовать конструктор по умолчанию,
тогда вы должны будете установить состояние в `StateMachine` с помощью метода `SwitchState`.

### 4. Привяжите переходы между состояниями

Мы можем добавить переходы между состояниями.
Для этого подойдут `StatefulTransitionSystem` и `IStateTransitions`.
C помощью методов `Add`, `AddAny`, а также методов расширения `At` и `AnyAt`, вы можете добавлять переходы.

| Метод                                                        | Описание                                                                |
|--------------------------------------------------------------|-------------------------------------------------------------------------|
| `Add(IState from, IStateTransition transition)`              | Принимает исходное состояние и переход                                  |
| `AddAny(IStateTransition transition)`                        | Принимает переход между состояниями                                     |
| `At(IState from, IState to, params Func<bool>[] conditions)` | Принимает исходное состояние, конечное состояние и условия для перехода |
| `AnyAt(IState to, params Func<bool>[] conditions)`           | Принимает конечное состояние и условия перехода                         |

Давайте представим ситуацию:

Мы хотим переключиться из состояния `AwaitingState` в состояние `FollowingState`, если цель найдена.
Также мы хотим переключиться обратно в состояние `AwaitingState`, если цель потеряна.
Метод `AddTransition` поможет нам в этом, давайте добавим два перехода:

Из состояния `AwaitingState` в состояние `FollowingState`

```csharp
transitions.Add(from: _awaitingState,
    new StateTransition(to: _followingState, condition: () => _target != null));
```

Также доступно такое расширение:

```csharp
transitions.At(from: _awaitingState, to: _followingState, () => _target != null);
```

Мы добавили переход из состояния `AwaitingState` в состояние `FollowingState` с условием, что цель не равна `null`.
Почему это нужно? 
Если цель равна `null`, то логично, что мы можем переключиться в состояние `FollowingState`.

### 5. Запустите StatefulTransitionSystem

Для запуска `StatefulTransitionSystem` вам нужно установить первое состояние и вызвать метод `Tick()` в `Update()`:

```csharp
private void Update() => _transitionSystem.Tick();
```