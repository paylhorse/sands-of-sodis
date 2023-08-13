-- ** A Character-Unit, representing the state of an individual
-- Backbone of the Character C# class

-- Define the CUnit class
CUnit = {}
CUnit.__index = CUnit

-- Constructor for CUnit
function CUnit.new(name, STR, RES, AGI, DEX, VAS, level)
    local self = setmetatable({}, CUnit)

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
    self.FEY = self.maxFey
    
    self.level = level or 1
    self.COMP = 0
    -- [Competence or XP]
    
    self.equipment = {}

    return self
end

-- Setter Functions
function CUnit:setVIT(value)
    if value < 0 then
        self.VIT = 0
    elseif value > self.maxVIT then
        self.VIT = self.maxVIT
    else
        self.VIT = value
    end
end

-- Other methods for the CUnit class
function CUnit:takeDamage(damage)
    self.VIT -= self.VIT
end

function CUnit:gainVIT(healing)
    if (self.VIT + healing) > self.maxVIT then
        self.VIT = self.maxVIT
    else
        self.VIT += self.VIT
    end
end

--[[ self.maxDET = (STR * 6 + RES * 6 + AGI * 6 + DEX * 6) / 4
self.DET = self.maxDET ]]--
