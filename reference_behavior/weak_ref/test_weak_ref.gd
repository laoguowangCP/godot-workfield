extends Node


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	var n = weakref(RefCounted.new())
	var n1 = n.get_ref()
	print(n.get_reference_count())


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
