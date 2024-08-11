extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	await get_parent().ready
	var owner_path = "null" if owner == null else String(owner.get_path())
	print(get_path(), ": ", owner_path)
	var child = Node.new()
	add_child(child)
	print(child.get_path(), ": ",
		"null" if child.owner == null else String(child.owner.get_path()))


func _process(_delta):
	pass
