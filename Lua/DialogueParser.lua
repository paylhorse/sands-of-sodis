DialogueParser = {}

function DialogueParser:parse(dialogueToml)
	local dialogueData = TOML.parse(dialogueToml)
	local scene = dialogueData["scene_001"]
	local dialogueSequence = DialogueParser:parseScene(scene, dialogueData)

	return dialogueSequence
end

function DialogueParser:parseScene(scene, dialogueData)
	local order = scene.order
	local dialogueSequence = {}

	for i, nodeName in ipairs(order) do
		local nodeData = dialogueData[nodeName]
		-- Parse Dialogue Nodes
		if nodeName == "Choice" then
			local choiceNode = nodeData -- Note this change
			if choiceNode.type == "multiple_choice" then
				local options = {}
				for _, option in ipairs(choiceNode.options) do
					local subSceneData = DialogueParser:parseScene(dialogueData[option.next_subscene], dialogueData)
					table.insert(options, { value = option.value, response = option.response, subscene = subSceneData })
				end
				table.insert(
					dialogueSequence,
					{ type = "multiple_choice", prompt = choiceNode.prompt, options = options }
				)
			end
		elseif nodeName == "Wait" then
			table.insert(dialogueSequence, { type = "wait", duration = nodeData.duration })
		else
			for idx, lineOrCommand in pairs(nodeData) do
				if string.match(idx, "line_") then
					local expressionIdx = "expression_" .. string.match(idx, "%d+")
					local expression = nodeData[expressionIdx]
					table.insert(
						dialogueSequence,
						{ type = "dialogue", character = nodeName, line = lineOrCommand, expression = expression }
					)
				elseif string.match(idx, "command_") then
					local action, parameter = string.match(lineOrCommand, "([^:]+):([^:]+)")
					table.insert(dialogueSequence, { type = "command", action = action, parameter = parameter })
				end
			end
		end
	end

	return dialogueSequence
end
