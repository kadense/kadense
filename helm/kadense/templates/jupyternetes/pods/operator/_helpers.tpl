{{- define "jupyternetes.pods.operator.fullname" -}}
{{- printf "jupyternetes-pods-op" }}
{{- end }}

{{- define "jupyternetes.pods.operator.serviceAccountName" -}}
{{- if ne .Values.jupyternetes.pods.operator.serviceAccount.name "" -}}
{{- printf "%s" .Values.jupyternetes.pods.operator.serviceAccount.name }}
{{- else -}}
{{- include "jupyternetes.pods.operator.fullname" . }}
{{- end }}
{{- end }}

{{- define "jupyternetes.pods.operator.image.repository" -}}
{{- if ne .Values.jupyternetes.pods.operator.image.repository "" -}}
{{- printf "%s" .Values.jupyternetes.pods.operator.image.repository }}
{{- else -}}
{{- printf "%s" .Values.image.repository }}
{{- end }}
{{- end }}


{{- define "jupyternetes.pod.status.operator.image.repository" -}}
{{- if ne .Values.jupyternetes.pod.status.operator.image.repository "" -}}
{{- printf "%s" .Values.jupyternetes.pod.status.operator.image.repository }}
{{- else -}}
{{- printf "%s" .Values.image.repository }}
{{- end }}
{{- end }}

{{- define "jupyternetes.pods.operator.image.tag" -}}
{{- if ne .Values.jupyternetes.pods.operator.image.tag "" -}}
{{- printf "%s" .Values.jupyternetes.pods.operator.image.tag }}
{{- else -}}
{{- printf "%s" .Values.image.tag }}
{{- end }}
{{- end }}

{{- define "jupyternetes.pod.status.operator.image.tag" -}}
{{- if ne .Values.jupyternetes.pod.status.operator.image.tag "" -}}
{{- printf "%s" .Values.jupyternetes.pod.status.operator.image.tag }}
{{- else -}}
{{- printf "%s" .Values.image.tag }}
{{- end }}
{{- end }}

{{- define "jupyternetes.pods.operator.image.name" -}}
{{- printf "%s/%s:%s" (include "jupyternetes.pods.operator.image.repository" .) .Values.jupyternetes.pods.operator.image.name (include "jupyternetes.pods.operator.image.tag" .) }}
{{- end }}


{{- define "jupyternetes.pod.status.operator.image.name" -}}
{{- printf "%s/%s:%s" (include "jupyternetes.pod.status.operator.image.repository" .) .Values.jupyternetes.pod.status.operator.image.name (include "jupyternetes.pod.status.operator.image.tag" .) }}
{{- end }}