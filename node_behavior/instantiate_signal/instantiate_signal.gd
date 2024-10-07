extends Node


var s : PackedScene = preload("res://node_behavior/instantiate_signal/to_be_instantiated.tscn")


func _init():
    pass

func _ready():
    var x = s.instantiate()
    var x_array = []
    x_array.append(x)
    var y = x_array[0].duplicate(1)
    self.add_child(y)
