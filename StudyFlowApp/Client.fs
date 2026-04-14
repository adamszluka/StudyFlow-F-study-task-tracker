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
            div [ attr.style "margin-bottom: 6px;" ] [
                text "Priority: "

                button [
                    on.click (fun _ _ -> priorityVar.Value <- Low)
                    attr.style (
                        if currentPriority = Low then
                            "margin-right: 6px; font-weight: bold;"
                        else
                            "margin-right: 6px;"
                    )
                ] [ text "Low" ]

                button [
                    on.click (fun _ _ -> priorityVar.Value <- Medium)
                    attr.style (
                        if currentPriority = Medium then
                            "margin-right: 6px; font-weight: bold;"
                        else
                            "margin-right: 6px;"
                    )
                ] [ text "Medium" ]

                button [
                    on.click (fun _ _ -> priorityVar.Value <- High)
                    attr.style (
                        if currentPriority = High then
                            "margin-right: 6px; font-weight: bold;"
                        else
                            "margin-right: 6px;"
                    )
                ] [ text "High" ]
            ]
        )

    let selectedPriorityText =
        priorityVar.View
        |> Doc.BindView (fun currentPriority ->
            div [ attr.style "margin-bottom: 10px;" ] [
                text ("Selected priority: " + priorityToString currentPriority)
            ]
        )

    let filterButtons =
        filterVar.View
        |> Doc.BindView (fun currentFilter ->
            div [ attr.style "margin-bottom: 20px;" ] [
                text "Filter: "

                button [
                    on.click (fun _ _ -> filterVar.Value <- All)
                    attr.style (
                        if currentFilter = All then
                            "margin-right: 6px; font-weight: bold;"
                        else
                            "margin-right: 6px;"
                    )
                ] [ text "All" ]

                button [
                    on.click (fun _ _ -> filterVar.Value <- Active)
                    attr.style (
                        if currentFilter = Active then
                            "margin-right: 6px; font-weight: bold;"
                        else
                            "margin-right: 6px;"
                    )
                ] [ text "Active" ]

                button [
                    on.click (fun _ _ -> filterVar.Value <- Done)
                    attr.style (
                        if currentFilter = Done then
                            "margin-right: 6px; font-weight: bold;"
                        else
                            "margin-right: 6px;"
                    )
                ] [ text "Done" ]
            ]
        )

    let taskList =
        View.Map2 (fun tasks filter -> tasks, filter) tasksVar.View filterVar.View
        |> Doc.BindView (fun (tasks, filter) ->
            tasks
            |> getFilteredTasks filter
            |> List.map (fun t ->
                div [ attr.style "margin-bottom: 10px;" ] [
                    span [
                        attr.style (
                            if t.IsDone then "text-decoration: line-through; margin-right: 10px;"
                            else "margin-right: 10px;"
                        )
                    ] [
                        text (t.Title + " | " + t.Subject + " | " + priorityToString t.Priority)
                    ]

                    button [
                        on.click (fun _ _ -> toggleTask t.Id)
                        attr.style "margin-right: 6px;"
                    ] [
                        text (if t.IsDone then "Undo" else "Done")
                    ]

                    button [
                        on.click (fun _ _ -> deleteTask t.Id)
                    ] [
                        text "Delete"
                    ]
                ]
            )
            |> Doc.Concat
        )

    div [] [
        h1 [] [ text "StudyFlow" ]

        h3 [] [ text "Add new task" ]

        div [ attr.style "margin-bottom: 6px;" ] [
            Doc.InputType.Text [ attr.placeholder "Task title" ] titleVar
        ]

        div [ attr.style "margin-bottom: 6px;" ] [
            Doc.InputType.Text [ attr.placeholder "Subject" ] subjectVar
        ]

        priorityButtons
        selectedPriorityText

        div [ attr.style "margin-bottom: 20px;" ] [
            button [
                on.click (fun _ _ -> addTask ())
            ] [ text "Add" ]
        ]

        filterButtons

        h3 [] [ text "Task list" ]

        div [] [
            taskList
        ]
    ]
    |> Doc.RunById "main"