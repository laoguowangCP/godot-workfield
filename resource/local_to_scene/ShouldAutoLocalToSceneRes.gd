@tool
class_name ShouldAutoLocalToSceneRes
extends Resource

var idx: int

func _init():
    set_local_to_scene(true)
    print("new ShouldAutoLocalToSceneRes");
