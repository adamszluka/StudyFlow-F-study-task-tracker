# StudyFlow

![.NET](https://img.shields.io/badge/.NET-10-blue)
![F#](https://img.shields.io/badge/F%23-functional-purple)
![WebSharper](https://img.shields.io/badge/WebSharper-SPA-green)
![Deployment](https://img.shields.io/badge/Live-Demo-success)

StudyFlow is a simple task management web application built with F# and WebSharper. The goal of the project is to help students manage their study-related tasks, subjects, and priorities in a clear and interactive way.

## Live Demo

https://studyflow-app-jlri.onrender.com/

---

## Motivation

As a university student, it is often difficult to track assignments, smaller tasks, and deadlines across multiple courses. This project was created to provide a lightweight study task manager with a simple interface and reactive user interactions.

---

## Features

- Add new tasks with title and subject
- Select task priority: Low, Medium, or High
- Mark tasks as done or undone
- Delete tasks
- Filter tasks by status: All, Active, Done
- View simple task statistics

---

## Technologies

- F#
- .NET 10
- WebSharper
- ASP.NET Core
- Docker (for deployment)

---

## Functional programming aspects

This project uses several functional programming concepts:

- immutable-style record updates
- discriminated unions for `Priority` and `Filter`
- list transformations with `List.map` and `List.filter`
- reactive state handling with `Var` and `View`

---

## Project Description

As part of my assignment, I developed a web application called StudyFlow using F# and the WebSharper framework. The goal of the project was to create a simple study management interface where users can organize their university-related tasks.

The application allows users to add new tasks, specify the subject, and set a priority level. Tasks can be marked as completed, reverted back to active, or deleted entirely. This provides a flexible and easy-to-use workflow for managing study-related activities.

An important feature of the application is task filtering. Users can display all tasks, only active tasks, or only completed tasks. In addition, the interface includes a simple statistics section that shows the total number of tasks, as well as the number of active and completed ones. These features not only improve usability but also demonstrate list-based operations in practice.

During implementation, several functional programming concepts were applied. Discriminated unions were used to represent priority levels and filtering states, while tasks are modeled using record types. State changes are handled using a reactive approach rather than traditional imperative programming. Task updates are implemented using `List.map` and `List.filter`, and UI reactivity is managed through WebSharper’s `Var` and `View` mechanisms.

Overall, StudyFlow is a simple but extensible application that demonstrates the combined use of F#, functional programming principles, and reactive web development.

---

## How to use

## Clone the repository

```bash
git clone https://github.com/adamszluka/StudyFlow-F-study-task-tracker.git
cd StudyFlow-F-study-task-tracker/StudyFlowApp```
---

```bash
dotnet build
dotnet run
Then open:
http://localhost:5000```


---

## Deployment

The application is deployed using Docker on Render.

```bash
Platform: Render
Runtime: Docker (.NET 10 + Node.js)
Live URL: https://studyflow-app-jlri.onrender.com/```