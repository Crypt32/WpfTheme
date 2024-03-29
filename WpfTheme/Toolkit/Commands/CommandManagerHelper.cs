using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Commands {
	static class CommandManagerHelper {
		internal static void CallWeakReferenceHandlers(List<WeakReference> handlers) {
			if (handlers != null) {
				// Take a snapshot of the handlers before we call out to them since the handlers
				// could cause the array to me modified while we are reading it.

				EventHandler[] callees = new EventHandler[handlers.Count];
				Int32 count = 0;

				for (Int32 i = handlers.Count - 1; i >= 0; i--) {
					WeakReference reference = handlers[i];
					EventHandler handler = reference.Target as EventHandler;
					if (handler == null) {
						// Clean up old handlers that have been collected
						handlers.RemoveAt(i);
					} else {
						callees[count] = handler;
						count++;
					}
				}

				// Call the handlers that we snapshotted
				for (Int32 i = 0; i < count; i++) {
					EventHandler handler = callees[i];
					handler(null, EventArgs.Empty);
				}
			}
		}
		internal static void AddHandlersToRequerySuggested(IEnumerable<WeakReference> handlers) {
			if (handlers != null) {
				foreach (EventHandler handler in handlers.Select(handlerRef => handlerRef.Target).OfType<EventHandler>()) {
					CommandManager.RequerySuggested += handler;
				}
			}
		}
		internal static void RemoveHandlersFromRequerySuggested(IEnumerable<WeakReference> handlers) {
			if (handlers != null) {
				foreach (WeakReference handlerRef in handlers) {
					EventHandler handler = handlerRef.Target as EventHandler;
					if (handler != null) {
						CommandManager.RequerySuggested -= handler;
					}
				}
			}
		}
		internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler) {
			AddWeakReferenceHandler(ref handlers, handler, -1);
		}
		internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, Int32 defaultListSize) {
			if (handlers == null) {
				handlers = (defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>());
			}

			handlers.Add(new WeakReference(handler));
		}
		internal static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler) {
			if (handlers != null) {
				for (Int32 i = handlers.Count - 1; i >= 0; i--) {
					WeakReference reference = handlers[i];
					EventHandler existingHandler = reference.Target as EventHandler;
					if ((existingHandler == null) || (existingHandler == handler)) {
						// Clean up old handlers that have been collected
						// in addition to the handler that is to be removed.
						handlers.RemoveAt(i);
					}
				}
			}
		}
	}
}