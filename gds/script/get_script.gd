class_name GetScript
extends Node

func _ready() -> void:
    var foo: Foo = get_node("Node")
    var s: Script = foo.get_script()
    print(s.get_script_property_list())

