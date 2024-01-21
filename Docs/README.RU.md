# Stateful

<div align="center">
    <strong><a href="README.md">English</a> | <a href="README.RU.md">Русский</a></strong>
</div>

<details>
<summary>Оглавление</summary>

- [Введение](#введение)
- [Установка](#установка)
- [Как использовать](#как-использовать)
    - [Реализуйте поле или свойство](#1-реализуйте-поле-или-свойство-машины-состояний)
    - [Определите состояния](#2-определите-состояния)
    - [Установите состояния в своем классе](#3-установите-состояния-в-своем-классе)
    - [Привяжите переходы между состояниями](#4-привяжите-переходы-между-состояниями)
    - [Запустите систему переходов](#5-запустите-систему-переходов)
- [Поддержка](#поддержка)
- [Лицензия](#лицензия)
</details>

## Введение

Проект предоставляет простую реализацию конечного автомата для использования **.Net** и **Unity**.

Конечные автоматы являются мощным инструментом для управления поведением объектов, состояниями и переходами между ними.

### Особенности:

* Поддержка переходов по условиям: Вы можете определить условия, при выполнении которых объект будет переходить между
  состояниями.
* Простое использование: Проект предоставляет простой **API** для определения состояний и их переходов, что делает его
  легким в использовании даже для новичков.
* Производительность: Реализация конечного автомата оптимизирована для высокой производительности, что делает его
  подходящим для использования в реальных проектах **Unity**.

### Содержание:

| Категория      | Описание                                                                                                    |
|----------------|-------------------------------------------------------------------------------------------------------------|
| `Abstract`     | **API** проекта, включающее интерфейсы `IState` и `IStateMachine`.                                          |
| `Finite`       | Простая реализация конечного автомата, предоставляющая базовую функциональность для управления состояниями. |
| `Transitional` | Поддержка переходов между состояниями по условиям.                                                          |

## Установка

### Ручная

Добавьте файл **.dll** из последнего релиза в свой проект.

### Через NuGet

Добавьте `Depra.Stateful` в свой проект с помощью **NuGet**.

## Как использовать

### 1. Реализуйте поле или свойство машины состояний

`TState` - некое состояние. Может быть любым типом, 
но реализация `StateMachine` работает с `IState`.

```csharp
private readonly IStateMachine<Sample> _stateMachine = new StateMachine<Sample>();
```

```csharp
public IStateMachine<Sample> StateMachine { get; } = new StateMachine<Sample>();
```

### 2. Определите состояния

Создайте состояния для объекта. 
В состояниях можно переопределить нужные вам методы: `Enter()`, `Exit()`.

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
Метод `Add` поможет нам в этом, давайте добавим два перехода:

Из состояния `AwaitingState` в состояние `FollowingState`:

```csharp
transitions.Add(from: _awaitingState,
    new StateTransition(to: _followingState, condition: () => _target != null));
```

Или с помощью методов расширения:

```csharp
transitions.At(from: _awaitingState, to: _followingState, () => _target != null);
```

Мы добавили переход из состояния `AwaitingState` в состояние `FollowingState` с условием, что цель не равна `null`.
Почему это нужно?
Если цель равна `null`, то логично, что мы можем переключиться в состояние `FollowingState`.

Собранную матрицу переходов нужно передать в `StatefulTransitionSystem` вместе с машиной состояний:

```csharp
_transitionSystem = new StatefulTransitionSystem(_stateMachine, transitions);
```

### 5. Запустите систему переходов

Для запуска `StatefulTransitionSystem` вам нужно установить первое состояние и вызвать метод `Tick()` в `Update()`:

```csharp
private void Update() => _transitionSystem.Tick();
```

## Поддержка

Я независимый разработчик,
и большая часть разработки этого проекта выполняется в свободное время.
Если вы заинтересованы в сотрудничестве или найме меня для проекта,
ознакомьтесь с моим [портфолио](https://github.com/Depra-Inc)
и [свяжитесь со мной](mailto:n.melnikov@depra.org)!

## Лицензия

**Apache-2.0**

Copyright (c) 2022-2024 Николай Мельников
[n.melnikov@depra.org](mailto:n.melnikov@depra.org)