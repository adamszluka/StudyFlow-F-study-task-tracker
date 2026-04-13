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

    let priorityButton label value =
        button [
            on.click (fun _ _ -> priorityVar.Value <- value)
            attr.style (
                if priorityVar.Value = value then
                    "margin-right: 6px; font-weight: bold;"
                else
                    "margin-right: 6px;"
            )
        ] [ text label ]

    div [] [
        h1 [] [ text "StudyFlow" ]

        h3 [] [ text "Add new task" ]

        div [ attr.style "margin-bottom: 6px;" ] [
            Doc.InputType.Text [ attr.placeholder "Task title" ] titleVar
        ]

        div [ attr.style "margin-bottom: 6px;" ] [
            Doc.InputType.Text [ attr.placeholder "Subject" ] subjectVar
        ]

        div [ attr.style "margin-bottom: 6px;" ] [
            text "Priority: "
            priorityButton "Low" Low
            priorityButton "Medium" Medium
            priorityButton "High" High
        ]

        div [ attr.style "margin-bottom: 10px;" ] [
            text ("Selected priority: " + priorityToString priorityVar.Value)
        ]

        div [ attr.style "margin-bottom: 20px;" ] [
            button [
                on.click (fun _ _ -> addTask ())
            ] [ text "Add" ]
        ]

        h3 [] [ text "Task list" ]

        div [] [
            tasksVar.View
            |> Doc.BindView (fun tasks ->
                tasks
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
        ]
    ]
    |> Doc.RunById "main"