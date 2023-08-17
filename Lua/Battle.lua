--                  .-.
--                 (   )
--                  '-'
--        .-"""-.
--      .`       `.
--     /  BATTLE   \
--     ;           ;
--     \           /
--      '.       .'
--        '-----'
--
-- Table to store serialized battles

Battle = {}
Battle.__index = Battle

-- Constructor for Battle table
function Battle.new(name, map, enterCondition, enemies)
	local self = setmetatable({}, Battle)

	self.name = name or "Unnamed"
	self.map = map
	self.enterCondition = enterCondition or "normal"
	self.enemies = enemies

	return self
end

return Battle
