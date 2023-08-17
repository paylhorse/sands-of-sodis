--                  .-.
--                 (   )
--                  '-'
--        .-"""-.
--      .`       `.
--     /  B(ATTLE) \
--     ;  UNIT     ;
--     \           /
--      '.       .'
--        '-----'
--
-- Units that populate the Battle scene

-- Define the BUnit class
BUnit = {}
BUnit.__index = BUnit

-- Constructor for BUnit
function BUnit.new(name, STR, RES, AGI, DEX, VAS, level)
	local self = setmetatable({}, BUnit)

	-- Info
	self.name = name or "Unnamed"
	self.class = {}

	-- Base Stats
	self.STR = STR or 10
	-- (Strength)
	self.RES = RES or 10
	-- (Resilience)
	self.AGI = AGI or 10
	-- (Agility)
	self.DEX = DEX or 10
	-- (Dexterity)
	self.VAS = VAS or 10
	-- (Vascularity)

	---------------------

	-- Derived Stats
	self.MOV = AGI / 4
	-- {Move Speed}
	self.ACT = (AGI / 3 + DEX / 2) / 2
	-- {Action Speed}
	self.maxVIT = RES * 10
	-- [Vitality]

	self.maxFey = VAS * 4
	-- [Fey]

	-- Status
	self.VIT = self.maxVIT
	self.Fey = self.maxFey

	self.level = level or 1
	self.COMP = 0
	-- [Competence or XP]

	self.equipment = {}

	return self
end

-- Setter Functions
function BUnit:setVIT(value)
	if value < 0 then
		self.VIT = 0
	elseif value > self.maxVIT then
		self.VIT = self.maxVIT
	else
		self.VIT = value
	end
end

-- Other methods for the BUnit class
function BUnit:takeDamage(damage)
	self.VIT = self.VIT - damage
end

function BUnit:gainVIT(healing)
	if (self.VIT + healing) > self.maxVIT then
		self.VIT = self.maxVIT
	else
		self.VIT = self.VIT + healing
	end
end

--[[ self.maxDET = (STR * 6 + RES * 6 + AGI * 6 + DEX * 6) / 4
self.DET = self.maxDET ]]
--

return BUnit
