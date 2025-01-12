/**
 * Copyright (c) 2019 LG Electronics, Inc.
 *
 * This software contains code licensed as described in LICENSE.
 *
 */

using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using Simulator.Controllable;

namespace Simulator.Api.Commands
{
    class ControllableGet : ICommand
    {
        public string Name => "controllable/get/all";

        public void Execute(JSONNode args)
        {
            var api = ApiManager.Instance;
            List<IControllable> controllables = SimulatorManager.Instance.Controllables;

            JSONArray result = new JSONArray();

            foreach (var controllable in controllables)
            {
                if (api.ControllablesUID.TryGetValue(controllable, out string uid))
                {
                    JSONArray validActions = new JSONArray();
                    foreach (var state in controllable.ValidStates)
                    {
                        validActions.Add(state);
                    }
                    foreach (var action in controllable.ValidActions)
                    {
                        validActions.Add(action);
                    }

                    JSONObject j = new JSONObject();
                    j.Add("uid", uid);
                    j.Add("position", controllable.transform.position);
                    j.Add("rotation", controllable.transform.rotation.eulerAngles);
                    j.Add("type", controllable.ControlType);
                    j.Add("valid_actions", validActions);
                    j.Add("default_control_policy", controllable.DefaultControlPolicy);
                    result.Add(j);
                }
            }

            api.SendResult(result);
        }
    }
}
