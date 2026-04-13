namespace StudyFlowApp

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating

[<JavaScript>]
module Client =
    // The templates are loaded from the DOM, so you just can edit index.html
    // and refresh your browser, no need to recompile unless you add or remove holes.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let People =
        ListModel.FromSeq [
            "John"
            "Paul"
        ]


[<SPAEntryPoint>]
let Main () =

    let input = ref ""

    let tasks = ref [
        "Math homework"
        "F# assignment"
    ]

    let render () =
        div [] [

            h1 [] [text "StudyFlow"]

            div [] [
                input [
                    attr.value !input
                    on.input (fun e -> input := e.Value)
                ]

                button [
                    on.click (fun _ ->
                        if !input <> "" then
                            tasks := !tasks @ [!input]
                            input := ""
                            render() |> ignore
                    )
                ] [text "Add"]
            ]

            ul [] (
                !tasks
                |> List.map (fun t -> li [] [text t])
            )
        ]

    render() |> ignore