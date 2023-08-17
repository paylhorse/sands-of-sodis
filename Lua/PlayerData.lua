--                  .-.
--                 (   )
--                  '-'
--        .-"""-.
--      .`       `.
--     /  PLAYER   \
--     ;  DATA     ;
--     \           /
--      '.       .'
--        '-----'
--
-- Represents the Player's current state and progress in the game

-- Define the PlayerData class
PlayerData = {}
PlayerData.__index = PlayerData

-- Constructor for PlayerData
function PlayerData.new()
	local self = setmetatable({}, PlayerData)

	self.party = {}
	self.inventory = {}
	self.coin = 0
	self.saveLocation = nil

	return self
end

-- Other methods for the PlayerData class
function PlayerData:addUnit(unit)
	table.insert(self.party, unit)
end

function PlayerData:getUnit(name)
	for _, unit in ipairs(self.party) do
		if unit.name == name then
			return unit
		end
	end
end

function PlayerData:gainCoin(profit)
	self.coin = self.coin + profit
end

function PlayerData:loseCoin(loss)
	self.coin = self.coin - loss
	if self.coin <= 0 then
		self.coin = 0
	end
end

return PlayerData
