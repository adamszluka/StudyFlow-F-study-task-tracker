module StudyFlowApp.Client

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript>]
type Priority =
    | Low
    | Medium
    | High

[<JavaScript>]
type Filter =
    | All
    | Active
    | Done

[<JavaScript>]
type StudyTask =
    {
        Id: int
        Title: string
        Subject: string
        Priority: Priority
        IsDone: bool
    }

[<JavaScript>]
let priorityToString p =
    match p with
    | Low -> "Low"
    | Medium -> "Medium"
    | High -> "High"

[<JavaScript>]
let priorityColor p =
    match p with
    | Low -> "#2e7d32"
    | Medium -> "#f9a825"
    | High -> "#c62828"

[<JavaScript; SPAEntryPoint>]
let Main () =
    let titleVar = Var.Create ""
    let subjectVar = Var.Create ""
    let priorityVar = Var.Create Medium
    let filterVar = Var.Create All

    let nextIdVar = Var.Create 3

    let tasksVar =
        Var.Create [
            {
                Id = 1
                Title = "Math homework"
                Subject = "Mathematics"
                Priority = High
                IsDone = false
            }
            {
                Id = 2
                Title = "F# assignment"
                Subject = "Functional Programming"
                Priority = Medium
                IsDone = false
            }
        ]

    let addTask () =
        let title = titleVar.Value.Trim()
        let subject = subjectVar.Value.Trim()
        let priority = priorityVar.Value

        if title <> "" && subject <> "" then
            let newTask =
                {
                    Id = nextIdVar.Value
                    Title = title
                    Subject = subject
                    Priority = priority
                    IsDone = false
                }

            tasksVar.Value <- tasksVar.Value @ [ newTask ]
            nextIdVar.Value <- nextIdVar.Value + 1
            titleVar.Value <- ""
            subjectVar.Value <- ""
            priorityVar.Value <- Medium

    let toggleTask taskId =
        tasksVar.Value <-
            tasksVar.Value
            |> List.map (fun t ->
                if t.Id = taskId then
                    { t with IsDone = not t.IsDone }
                else
                    t
            )

    let deleteTask taskId =
        tasksVar.Value <-
            tasksVar.Value
            |> List.filter (fun t -> t.Id <> taskId)

    let getFilteredTasks filter tasks =
        match filter with
        | All -> tasks
        | Active -> tasks |> List.filter (fun t -> not t.IsDone)
        | Done -> tasks |> List.filter (fun t -> t.IsDone)

    let priorityButtons =
        priorityVar.View
        |> Doc.BindView (fun currentPriority ->
            div [ attr.style "margin-bottom: 12px;" ] [
                div [ attr.style "font-weight: bold; margin-bottom: 6px;" ] [
                    text "Priority"
                ]

                button [
                    on.click (fun _ _ -> priorityVar.Value <- Low)
                    attr.style (
                        if currentPriority = Low then
                            "margin-right: 8px; padding: 8px 14px; border-radius: 8px; border: 1px solid #2e7d32; background: #e8f5e9; font-weight: bold; cursor: pointer;"
                        else
                            "margin-right: 8px; padding: 8px 14px; border-radius: 8px; border: 1px solid #cccccc; background: white; cursor: pointer;"
                    )
                ] [ text "Low" ]

                button [
                    on.click (fun _ _ -> priorityVar.Value <- Medium)
                    attr.style (
                        if currentPriority = Medium then
                            "margin-right: 8px; padding: 8px 14px; border-radius: 8px; border: 1px solid #f9a825; background: #fff8e1; font-weight: bold; cursor: pointer;"
                        else
                            "margin-right: 8px; padding: 8px 14px; border-radius: 8px; border: 1px solid #cccccc; background: white; cursor: pointer;"
                    )
                ] [ text "Medium" ]

                button [
                    on.click (fun _ _ -> priorityVar.Value <- High)
                    attr.style (
                        if currentPriority = High then
                            "margin-right: 8px; padding: 8px 14px; border-radius: 8px; border: 1px solid #c62828; background: #ffebee; font-weight: bold; cursor: pointer;"
                        else
                            "margin-right: 8px; padding: 8px 14px; border-radius: 8px; border: 1px solid #cccccc; background: white; cursor: pointer;"
                    )
                ] [ text "High" ]
            ]
        )

    let selectedPriorityText =
        priorityVar.View
        |> Doc.BindView (fun currentPriority ->
            div [ attr.style "margin-bottom: 16px; color: #444;" ] [
                text ("Selected priority: " + priorityToString currentPriority)
            ]
        )

    let filterButtons =
        filterVar.View
        |> Doc.BindView (fun currentFilter ->
            div [ attr.style "margin-bottom: 20px;" ] [
                span [ attr.style "font-weight: bold; margin-right: 10px;" ] [
                    text "Filter"
                ]

                button [
                    on.click (fun _ _ -> filterVar.Value <- All)
                    attr.style (
                        if currentFilter = All then
                            "margin-right: 8px; padding: 8px 14px; border-radius: 8px; border: none; background: #1976d2; color: white; font-weight: bold; cursor: pointer;"
                        else
                            "margin-right: 8px; padding: 8px 14px; border-radius: 8px; border: 1px solid #cccccc; background: white; cursor: pointer;"
                    )
                ] [ text "All" ]

                button [
                    on.click (fun _ _ -> filterVar.Value <- Active)
                    attr.style (
                        if currentFilter = Active then
                            "margin-right: 8px; padding: 8px 14px; border-radius: 8px; border: none; background: #1976d2; color: white; font-weight: bold; cursor: pointer;"
                        else
                            "margin-right: 8px; padding: 8px 14px; border-radius: 8px; border: 1px solid #cccccc; background: white; cursor: pointer;"
                    )
                ] [ text "Active" ]

                button [
                    on.click (fun _ _ -> filterVar.Value <- Done)
                    attr.style (
                        if currentFilter = Done then
                            "padding: 8px 14px; border-radius: 8px; border: none; background: #1976d2; color: white; font-weight: bold; cursor: pointer;"
                        else
                            "padding: 8px 14px; border-radius: 8px; border: 1px solid #cccccc; background: white; cursor: pointer;"
                    )
                ] [ text "Done" ]
            ]
        )

    let statsPanel =
        View.Map (fun tasks ->
            let total = List.length tasks
            let doneCount = tasks |> List.filter (fun t -> t.IsDone) |> List.length
            let activeCount = total - doneCount

            div [
                attr.style "display: flex; gap: 12px; margin-bottom: 20px; flex-wrap: wrap;"
            ] [
                div [ attr.style "padding: 12px 18px; border-radius: 10px; background: #f5f5f5; min-width: 110px;" ] [
                    div [ attr.style "font-size: 12px; color: #666;" ] [ text "Total" ]
                    div [ attr.style "font-size: 22px; font-weight: bold;" ] [ text (string total) ]
                ]
                div [ attr.style "padding: 12px 18px; border-radius: 10px; background: #e8f5e9; min-width: 110px;" ] [
                    div [ attr.style "font-size: 12px; color: #666;" ] [ text "Active" ]
                    div [ attr.style "font-size: 22px; font-weight: bold;" ] [ text (string activeCount) ]
                ]
                div [ attr.style "padding: 12px 18px; border-radius: 10px; background: #e3f2fd; min-width: 110px;" ] [
                    div [ attr.style "font-size: 12px; color: #666;" ] [ text "Done" ]
                    div [ attr.style "font-size: 22px; font-weight: bold;" ] [ text (string doneCount) ]
                ]
            ]
        ) tasksVar.View
        |> Doc.EmbedView

    let taskList =
        View.Map2 (fun tasks filter -> tasks, filter) tasksVar.View filterVar.View
        |> Doc.BindView (fun (tasks, filter) ->
            let filtered = getFilteredTasks filter tasks

            if List.isEmpty filtered then
                div [
                    attr.style "padding: 18px; border-radius: 10px; background: #fafafa; color: #666;"
                ] [
                    text "No tasks in this view."
                ]
            else
                filtered
                |> List.map (fun t ->
                    div [
                        attr.style "margin-bottom: 12px; padding: 14px; border: 1px solid #e0e0e0; border-radius: 12px; background: white; box-shadow: 0 1px 3px rgba(0,0,0,0.06);"
                    ] [
                        div [ attr.style "display: flex; justify-content: space-between; align-items: center; gap: 12px; flex-wrap: wrap;" ] [
                            div [] [
                                div [
                                    attr.style (
                                        if t.IsDone then
                                            "font-size: 18px; font-weight: bold; text-decoration: line-through; color: #777; margin-bottom: 4px;"
                                        else
                                            "font-size: 18px; font-weight: bold; margin-bottom: 4px;"
                                    )
                                ] [
                                    text t.Title
                                ]

                                div [ attr.style "color: #555; margin-bottom: 6px;" ] [
                                    text ("Subject: " + t.Subject)
                                ]

                                span [
                                    attr.style ("display: inline-block; padding: 4px 10px; border-radius: 999px; font-size: 12px; font-weight: bold; color: white; background: " + priorityColor t.Priority + ";")
                                ] [
                                    text (priorityToString t.Priority)
                                ]
                            ]

                            div [] [
                                button [
                                    on.click (fun _ _ -> toggleTask t.Id)
                                    attr.style "margin-right: 8px; padding: 8px 12px; border-radius: 8px; border: none; background: #424242; color: white; cursor: pointer;"
                                ] [
                                    text (if t.IsDone then "Undo" else "Done")
                                ]

                                button [
                                    on.click (fun _ _ -> deleteTask t.Id)
                                    attr.style "padding: 8px 12px; border-radius: 8px; border: none; background: #d32f2f; color: white; cursor: pointer;"
                                ] [
                                    text "Delete"
                                ]
                            ]
                        ]
                    ]
                )
                |> Doc.Concat
        )

    div [
        attr.style "max-width: 900px; margin: 40px auto; padding: 24px; font-family: Arial, sans-serif; background: #fcfcfc;"
    ] [
        div [
            attr.style "background: linear-gradient(135deg, #1976d2, #42a5f5); color: white; padding: 24px; border-radius: 16px; margin-bottom: 24px;"
        ] [
            h1 [ attr.style "margin: 0 0 8px 0;" ] [ text "StudyFlow" ]
            p [ attr.style "margin: 0; font-size: 16px;" ] [
                text "A simple study task manager built with F# and WebSharper."
            ]
        ]

        statsPanel

        div [
            attr.style "background: white; padding: 20px; border-radius: 14px; box-shadow: 0 1px 4px rgba(0,0,0,0.08); margin-bottom: 24px;"
        ] [
            h3 [ attr.style "margin-top: 0;" ] [ text "Add new task" ]

            div [ attr.style "margin-bottom: 10px;" ] [
                Doc.InputType.Text [
                    attr.placeholder "Task title"
                    attr.style "width: 100%; padding: 10px; border-radius: 8px; border: 1px solid #cccccc; box-sizing: border-box;"
                ] titleVar
            ]

            div [ attr.style "margin-bottom: 10px;" ] [
                Doc.InputType.Text [
                    attr.placeholder "Subject"
                    attr.style "width: 100%; padding: 10px; border-radius: 8px; border: 1px solid #cccccc; box-sizing: border-box;"
                ] subjectVar
            ]

            priorityButtons
            selectedPriorityText

            div [] [
                button [
                    on.click (fun _ _ -> addTask ())
                    attr.style "padding: 10px 16px; border-radius: 8px; border: none; background: #1976d2; color: white; font-weight: bold; cursor: pointer;"
                ] [ text "Add task" ]
            ]
        ]

        filterButtons

        h3 [] [ text "Task list" ]

        div [] [
            taskList
        ]
    ]
    |> Doc.RunById "main"